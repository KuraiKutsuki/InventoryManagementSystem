using System;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Managers;

namespace InventoryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Inventory Manager...");

            try
            {
                InventoryManager inventory = new InventoryManager();

                // Test Adding Category
                inventory.AddCategory("Electronics");
                inventory.AddCategory("Furniture");

                // Test Adding Supplier
                inventory.AddSupplier("TechCorp", "contact@techcorp.com");

                Console.WriteLine("\n--- Current Categories ---");
                foreach (var cat in inventory.GetCategories())
                {
                    Console.WriteLine($"ID: {cat.Id}, Name: {cat.Name}");
                }

                Console.WriteLine("\n--- Current Suppliers ---");
                foreach (var sup in inventory.GetSuppliers())
                {
                    Console.WriteLine($"ID: {sup.Id}, Name: {sup.Name}, Contact: {sup.ContactInfo}");
                }

                Console.WriteLine("\nManager compiled and tested successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
