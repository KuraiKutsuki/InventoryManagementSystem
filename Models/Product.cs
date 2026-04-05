namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int CategoryId { get; private set; }
        public int SupplierId { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public int LowStockThreshold { get; private set; }

        public Product(int id, string name, int categoryId, int supplierId, decimal price, int stockQuantity, int lowStockThreshold)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            SupplierId = supplierId;
            Price = price;
            StockQuantity = stockQuantity;
            LowStockThreshold = lowStockThreshold;
        }

        // Methods to modify stock ensuring encapsulation
        public void AddStock(int amount)
        {
            if (amount > 0) StockQuantity += amount;
        }

        public void DeductStock(int amount)
        {
            if (amount > 0 && StockQuantity >= amount) StockQuantity -= amount;
        }

        public void UpdateDetails(string name, decimal price, int threshold)
        {
            Name = name;
            Price = price;
            LowStockThreshold = threshold;
        }
    }
}
