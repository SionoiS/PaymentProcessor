using System.ComponentModel.DataAnnotations;

namespace PaymentProcessor.Models
{
    public partial class Notification
    {
        public Purchase Purchase { get; set; }
        public Transaction Transaction { get; set; }
        //public PaymentDetails Payment_Details { get; set; }
    }

    public class Purchase
    {
        public VirtualCurrency Virtual_Currency { get; set; }
        //public Checkout Checkout { get; set; }
        //public Subscription Subscription { get; set; }
        //public VirtualItems Virtual_Items { get; set; }
        //public Total Total { get; set; }
        //public Promotion[] Promotions { get; set; }
        //public Coupon Coupon { get; set; }
    }

    public class VirtualCurrency
    {
        //public string Name { get; set; }
        //public string Sku { get; set; }
        public double Quantity { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
    }

    /*public class Checkout
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class Subscription
    {
        public string Plan_Id { get; set; }
        public long Subscription_Id { get; set; }
        public string Product_Id { get; set; }
        public string[] Tags { get; set; }
        public DateTimeOffset Date_Create { get; set; }
        public DateTimeOffset Date_Next_Charge { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class VirtualItems
    {
        public Item[] Items { get; set; }
        public string Currency { get; set; }
        public long Amount { get; set; }
    }*/

    /*public class Item
    {
        public string Sku { get; set; }
        public long Amount { get; set; }
    }*/

    /*public class Total
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class Promotion
    {
        public string Technical_Name { get; set; }
        public long Id { get; set; }
    }*/

    /*public class Coupon
    {
        public string Coupon_Code { get; set; }
        public string Campaign_Code { get; set; }
    }*/

    public class Transaction
    {
        [Required]
        public long Id { get; set; }
        //public string External_Id { get; set; }
        //public DateTimeOffset Payment_Date { get; set; }
        //public long Payment_Method { get; set; }
        //public long Dry_Run { get; set; }
        //public long Agreement { get; set; }
    }

    /*public class PaymentDetails
    {
        public Payment Payment { get; set; }
        public Vat Vat { get; set; }
        public double Payout_Currency_Rate { get; set; }
        public Payout Payout { get; set; }
        public XsollaFee Xsolla_Fee { get; set; }
        public PaymentMethodFee Payment_Method_Fee { get; set; }
        public RepatriationCommission Repatriation_Commission { get; set; }
    }*/

    /*public class Payout
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class Payment
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class Vat
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class XsollaFee
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class PaymentMethodFee
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/

    /*public class RepatriationCommission
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }*/
}
