namespace PaymentProcessor.Models
{
    public class ErrorResponse
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }

        public string message { get; set; }
    }
}
