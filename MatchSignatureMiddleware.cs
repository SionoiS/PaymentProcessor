using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PaymentProcessor.Models;
using System;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentProcessor
{
    public class MatchSignatureMiddleware
    {
        readonly RequestDelegate next;

        readonly ILogger<MatchSignatureMiddleware> logger;

        readonly SHA1 SHA1;

        readonly string secretKey;

        public MatchSignatureMiddleware(RequestDelegate next, ILogger<MatchSignatureMiddleware> logger)
        {
            SHA1 = SHA1.Create();

            secretKey = Environment.GetEnvironmentVariable("WEBHOOK_SECRET_KEY");

            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogDebug("Verifying Signature");

            var request = context.Request;
            request.EnableBuffering();//buff it cause it's used multiple times

            string bodyString;

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyString = await reader.ReadToEndAsync();
                request.Body.Position = 0;//rewind
            }

            //Concatenate the request's JSON body with your project's secret key
            bodyString = string.Concat(bodyString, secretKey);

            if (!request.Headers.TryGetValue("Authorization", out var headerValue))
            {
                logger.LogDebug("No authorization header, match signature failed");
                context.Response.StatusCode = 401;
                return;
            }

            string headerSignature = headerValue[0].Split(" ")[1];//"Bearer <signature>" extract signature
            
            //Apply SHA-1 hashing to the result.
            byte[] data = SHA1.ComputeHash(Encoding.UTF8.GetBytes(bodyString));
            var signatureHash = BitConverter.ToString(data).Replace("-", "");

            //Make sure that the created signature matches the one passed in the HTTP header.
            if (!string.Equals(signatureHash, headerSignature, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogDebug("Match signature failed " + signatureHash + " != " + headerSignature);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json; charset=utf-8";

                var err = new ErrorResponse
                {
                    error = new Error
                    {
                        code = "INVALID_SIGNATURE",
                        message = "Invalid Signature"
                    }
                };

                await JsonSerializer.SerializeAsync(context.Response.Body, err);

                await context.Response.Body.DisposeAsync();

                return;
            }

            await next(context);
        }
     }
}
