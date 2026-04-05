using System;

namespace InventoryManagementSystem.Models
{
    public class TransactionRecord
    {
        public int TransactionId { get; private set; }
        public int ProductId { get; private set; }
        public string TransactionType { get; private set; } // e.g., "Add", "Restock", "Deduct"
        public int Quantity { get; private set; }
        public string PerformedBy { get; private set; }
        public string Role { get; private set; }
        public DateTime Date { get; private set; }

        public TransactionRecord(int transactionId, int productId, string transactionType, int quantity, string performedBy, string role)
        {
            TransactionId = transactionId;
            ProductId = productId;
            TransactionType = transactionType;
            Quantity = quantity;
            PerformedBy = performedBy;
            Role = role;
            Date = DateTime.Now;
        }
    }
}
