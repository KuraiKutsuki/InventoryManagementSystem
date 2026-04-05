using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Managers
{
    public class InventoryManager
    {
        // Encapsulated lists to prevent external modification
        private List<Category> _categories;
        private List<Supplier> _suppliers;
        private List<Product> _products;
        private List<User> _users;
        private List<TransactionRecord> _transactions;

        // Auto-incrementing IDs
        private int _nextCategoryId = 1;
        private int _nextSupplierId = 1;
        private int _nextProductId = 1;
        private int _nextTransactionId = 1;

        public InventoryManager()
        {
            _categories = new List<Category>();
            _suppliers = new List<Supplier>();
            _products = new List<Product>();
            _users = new List<User>();
            _transactions = new List<TransactionRecord>();
        }

        // --- Core Functionality: Add Category ---
        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            Category newCategory = new Category(_nextCategoryId++, name);
            _categories.Add(newCategory);
            Console.WriteLine($"[Success] Category '{name}' added successfully with ID {newCategory.Id}.");
        }

        public IReadOnlyList<Category> GetCategories()
        {
            return _categories.AsReadOnly();
        }

        // --- Core Functionality: Add Supplier ---
        public void AddSupplier(string name, string contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contactInfo))
            {
                throw new ArgumentException("Supplier name and contact info cannot be empty.");
            }

            Supplier newSupplier = new Supplier(_nextSupplierId++, name, contactInfo);
            _suppliers.Add(newSupplier);
            Console.WriteLine($"[Success] Supplier '{name}' added successfully with ID {newSupplier.Id}.");
        }

        public IReadOnlyList<Supplier> GetSuppliers()
        {
            return _suppliers.AsReadOnly();
        }

        // --- Core Functionality: Product Management ---
        public void AddProduct(string name, int categoryId, int supplierId, decimal price, int stockQuantity, int lowStockThreshold)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");
            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            // Validate Category and Supplier exist
            if (!_categories.Any(c => c.Id == categoryId))
                throw new Exception($"Category with ID {categoryId} does not exist.");
            if (!_suppliers.Any(s => s.Id == supplierId))
                throw new Exception($"Supplier with ID {supplierId} does not exist.");

            Product newProduct = new Product(_nextProductId++, name, categoryId, supplierId, price, stockQuantity, lowStockThreshold);
            _products.Add(newProduct);
            Console.WriteLine($"[Success] Product '{name}' added successfully with ID {newProduct.Id}.");
        }

        public IReadOnlyList<Product> GetProducts()
        {
            return _products.AsReadOnly();
        }

        public IReadOnlyList<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetProducts();
            
            return _products
                .Where(p => p.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList()
                .AsReadOnly();
        }

        public void UpdateProduct(int id, string newName, decimal newPrice, int newThreshold)
        {
            Product? product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Product name cannot be empty.");
            if (newPrice < 0)
                throw new ArgumentException("Price cannot be negative.");

            product.UpdateDetails(newName, newPrice, newThreshold);
            Console.WriteLine($"[Success] Product ID {id} updated successfully.");
        }

        public void DeleteProduct(int id)
        {
            Product? product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            _products.Remove(product);
            Console.WriteLine($"[Success] Product ID {id} ('{product.Name}') deleted successfully.");
        }

        // --- Core Functionality: Inventory & Transactions ---
        public void RestockProduct(int productId, int amount)
        {
            Product? product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null) throw new Exception($"Product with ID {productId} not found.");
            if (amount <= 0) throw new ArgumentException("Restock amount must be greater than zero.");

            product.AddStock(amount);
            
            // Record transaction
            _transactions.Add(new TransactionRecord(_nextTransactionId++, productId, "Restock", amount));
            Console.WriteLine($"[Success] Restocked {amount} units to '{product.Name}'. New Stock: {product.StockQuantity}");
        }

        public void DeductProduct(int productId, int amount)
        {
            Product? product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null) throw new Exception($"Product with ID {productId} not found.");
            if (amount <= 0) throw new ArgumentException("Deduct amount must be greater than zero.");
            if (product.StockQuantity < amount) throw new InvalidOperationException($"Insufficient stock. Current stock is {product.StockQuantity}.");

            product.DeductStock(amount);

            // Record transaction
            _transactions.Add(new TransactionRecord(_nextTransactionId++, productId, "Deduct", amount));
            Console.WriteLine($"[Success] Deducted {amount} units from '{product.Name}'. New Stock: {product.StockQuantity}");
        }

        public IReadOnlyList<TransactionRecord> GetTransactions()
        {
            return _transactions.AsReadOnly();
        }

        public IReadOnlyList<Product> GetLowStockProducts()
        {
            return _products.Where(p => p.StockQuantity <= p.LowStockThreshold).ToList().AsReadOnly();
        }

        public decimal GetTotalInventoryValue()
        {
            return _products.Sum(p => p.Price * p.StockQuantity);
        }
    }
}
