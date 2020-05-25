using System.ComponentModel.DataAnnotations;

namespace PaymentProcessor.Models
{
    public partial class Notification
    {
        [Required]
        public string Notification_Type { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        //public string Ip { get; set; }

        //public string Phone { get; set; }

        //public string Email { get; set; }

        [Required]
        public string Id { get; set; }

        //public string Name { get; set; }

        //public string Country { get; set; }
    }
}
