using System;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Models...");

            try
            {
                Product testProduct = new Product(1, "Laptop", 101, 201, 999.99m, 10, 5);
                Console.WriteLine($"Product created: {testProduct.Name}, Stock: {testProduct.StockQuantity}");
                
                testProduct.AddStock(5);
                Console.WriteLine($"Stock after restock: {testProduct.StockQuantity}");

                Console.WriteLine("All models compiled and tested successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
