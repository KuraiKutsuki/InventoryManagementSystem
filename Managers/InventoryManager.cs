using System;
using System.Collections.Generic;
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
    }
}
