namespace PaymentProcessor.Models
{
    public partial class Notification
    {
        public RefundDetails Refund_Details { get; set; }
    }

    public class RefundDetails
    {
        public long Code { get; set; }
        /*  Codes
        1.	Cancellation by the user request / the game request.	Cancellation initiated from Publisher Account.
        2.	Chargeback.	Transaction chargeback requested.
        3.	Integration Error.	Issues in integration between Xsolla and the game. Recommendation: Do not blacklist the user.
        4.	Fraud.	Fraud suspected.
        5.	Test Payment.	Test transaction followed by cancellation. Recommendation: Do not blacklist the user.
        6.	Expired Invoice.	Invoice overdue (used for postpaid model).
        7.	PS debt cancel.	Payout refused by payment system. Recommendation: Do not blacklist the user.
        8.	Cancellation by the PS request.	Cancellation requested by payment system. Recommendation: Do not blacklist the user.
        9.	Cancellation by the user request.	The user was not satisfied with the game or the purchase for any reason. Recommendation: Do not blacklist the user.
        10.	Cancellation by the game request.	Cancellation requested by the game. Recommendation: Do not blacklist the user.
        11.	Account holder called to report fraud.	The account owner states that they didn't make the transaction.
        12.	Friendly fraud.	Friendly fraud reported.
     */
        //public string Reason { get; set; }
        //public string Author { get; set; }
    }
}
