using Google.Cloud.Firestore;

namespace PaymentProcessor
{
    public interface IFirestore
    {
        CollectionReference UserCollection { get; }
    }

    public class FirestoreService : IFirestore
    {
        readonly FirestoreDb FirestoreDb;

        public CollectionReference UserCollection { get; }

        public FirestoreService()
        {
            FirestoreDb = FirestoreDb.Create("roguefleetonline");

            UserCollection = FirestoreDb.Collection("users");

            //other collection possibly?
        }
    }
    
    /*[FirestoreData]
    public class TransactionFirestoreData
    {
        [FirestoreProperty]
        public DateTime Date { get; set; }

        [FirestoreProperty]
        public string Currency { get; set; }

        [FirestoreProperty]
        public double Cost { get; set; }

        [FirestoreProperty]
        public double Quantity { get; set; }
    }*/
}
