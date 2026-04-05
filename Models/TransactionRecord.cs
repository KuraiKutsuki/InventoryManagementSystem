using System;

namespace InventoryManagementSystem.Models
{
    public class TransactionRecord
    {
        public int TransactionId { get; private set; }
        public int ProductId { get; private set; }
        public string TransactionType { get; private set; } // e.g., "Add", "Restock", "Deduct"
        public int Quantity { get; private set; }
        public DateTime Date { get; private set; }

        public TransactionRecord(int transactionId, int productId, string transactionType, int quantity)
        {
            TransactionId = transactionId;
            ProductId = productId;
            TransactionType = transactionType;
            Quantity = quantity;
            Date = DateTime.Now;
        }
    }
}
