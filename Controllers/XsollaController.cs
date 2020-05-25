using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentProcessor.Models;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PaymentProcessor.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class XsollaController : ControllerBase
    {
        const string TransactionCollection = "transact";
        const string CreditsField = "credits";
        const string Notification_Type_User_Validation = "user_validation";
        const string Notification_Type_Payment = "payment";
        const string Notification_Type_Refund = "refund";

        readonly IFirestore firestoreService;

        readonly ILogger<XsollaController> logger;

        public XsollaController(IFirestore firestoreService, ILogger<XsollaController> logger)
        {
            this.firestoreService = firestoreService;
            this.logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WebHook(Notification notification)
        {
            if (notification.Notification_Type == Notification_Type_User_Validation)
            {
                return await ValidateUser(notification);
            }
            else if (notification.Notification_Type == Notification_Type_Payment)
            {
                return await ProcessPayment(notification);
            }
            else if (notification.Notification_Type == Notification_Type_Refund)
            {
                return await ProcessRefund(notification);
            }

            return BadRequest();
        }

        async Task<IActionResult> ValidateUser(Notification notification)
        {
            logger.LogDebug("Validate User");

            var userRef = firestoreService.UserCollection.Document(notification.User.Id);

            logger.LogDebug("User Id {0}", userRef.Id);

            var snapshot = await userRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                logger.LogDebug("User Valid");
                return Ok();
            }
            else
            {
                logger.LogDebug("User Invalid");

                var err = new ErrorResponse
                {
                    error = new Error
                    {
                        code = "INVALID_USER",
                        message = "Invalid User"
                    }
                };

                return BadRequest(err);
            }
        }

        async Task<IActionResult> ProcessPayment(Notification notification)
        {
            logger.LogDebug("Process Payment");

            var userRef = firestoreService.UserCollection.Document(notification.User.Id);

            logger.LogDebug("User Id {0}", userRef.Id);

            var transactionRef = userRef.Collection(TransactionCollection).Document(notification.Transaction.Id.ToString());

            logger.LogDebug("Transaction Id {0}", transactionRef.Id);

            var userSnapshot = await userRef.GetSnapshotAsync();
            var transactionSnapshot = await transactionRef.GetSnapshotAsync();

            if (userSnapshot.Exists && !transactionSnapshot.Exists)
            {
                logger.LogDebug("Adding New Transaction!");

                var data = new
                {
                    Date = FieldValue.ServerTimestamp,
                    Currency = notification.Purchase.Virtual_Currency.Currency,
                    Cost = notification.Purchase.Virtual_Currency.Amount,
                    Quantity = notification.Purchase.Virtual_Currency.Quantity
                };

                logger.LogDebug("Currency {0} Cost {1} Quantity {2}", data.Currency, data.Cost, data.Quantity);

                await transactionRef.CreateAsync(data);
                await userRef.UpdateAsync(CreditsField, FieldValue.Increment(data.Quantity));
            }
            else
            {
                logger.LogDebug("Can't Process Transaction Twice!");
            }

            return Ok();
        }

        async Task<IActionResult> ProcessRefund(Notification notification)
        {
            logger.LogDebug("Process Refund");

            var userRef = firestoreService.UserCollection.Document(notification.User.Id);

            logger.LogDebug("User Id {0}", userRef.Id);

            var transactionRef = userRef.Collection(TransactionCollection).Document(notification.Transaction.Id.ToString());

            logger.LogDebug("Transaction Id {0}", transactionRef.Id);

            var userSnapshot = await userRef.GetSnapshotAsync();
            var transactionSnapshot = await transactionRef.GetSnapshotAsync();

            if (userSnapshot.Exists && transactionSnapshot.Exists)
            {
                logger.LogDebug("Refunding Transaction!");

                var quantity = transactionSnapshot.GetValue<double>("Quantity");

                await transactionRef.SetAsync(new { RefundDate = FieldValue.ServerTimestamp, RefundCode = notification.Refund_Details.Code }, SetOptions.MergeAll);
                await userRef.UpdateAsync(CreditsField, FieldValue.Increment(-quantity));
            }
            else
            {
                logger.LogDebug("Can't Refund Non-Existing Transaction!");
            }

            return Ok();
        }
    }
}
