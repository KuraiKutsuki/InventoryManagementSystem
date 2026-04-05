using System;
using System.Linq;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Managers;

namespace InventoryManagementSystem
{
    class Program
    {
        const int WIDTH = 60; // inner width between the border pipes

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InventoryManager inventory = new InventoryManager();

            // --- User Login at Startup ---
            Console.Clear();
            PrintHeader("SYSTEM LOGIN");
            string username = GetValidString("  Enter your Username: ");
            string role = GetValidString("  Enter your Role (e.g., Admin, Staff): ");
            inventory.SetCurrentUser(username, role);
            Console.WriteLine($"\n  Welcome, {username} ({role})! Press Enter to continue to the main menu...");
            Console.ReadLine();

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                TopBorder();
                EmptyRow();
                CenteredRow("INVENTORY MANAGEMENT SYSTEM");
                EmptyRow();
                SectionDivider();
                EmptyRow();
                CenteredRow("[ MAIN MENU ]");
                EmptyRow();
                ThinDivider();
                EmptyRow();
                MenuRow("1", "Add Category");
                MenuRow("2", "Add Supplier");
                MenuRow("3", "Add Product");
                MenuRow("4", "View All Products");
                MenuRow("5", "Search Product");
                MenuRow("6", "Update Product");
                MenuRow("7", "Delete Product");
                MenuRow("8", "Restock / Deduct Stock");
                MenuRow("9", "Inventory Reports");
                EmptyRow();
                ThinDivider();
                EmptyRow();
                MenuRow("0", "Exit");
                EmptyRow();
                BottomBorder();
                Console.WriteLine();
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            string catName = GetValidString("Enter Category Name (type 'cancel' to abort): ");
                            inventory.AddCategory(catName);
                            break;

                        case "2":
                            string supName = GetValidString("Enter Supplier Name (type 'cancel' to abort): ");
                            string supContact = GetValidString("Enter Supplier Contact Info: ");
                            inventory.AddSupplier(supName, supContact);
                            break;

                        case "3":
                            if (!inventory.GetCategories().Any() || !inventory.GetSuppliers().Any())
                            {
                                Console.WriteLine("[Error] You must add at least one Category and one Supplier before adding a product.");
                                break;
                            }

                            string prodName = GetValidString("Enter Product Name (type 'cancel' to abort): ");
                            
                            Console.WriteLine("\n--- Available Categories ---");
                            foreach (var cat in inventory.GetCategories())
                            {
                                Console.WriteLine($"  [ID: {cat.Id, -2}] {cat.Name}");
                            }
                            Console.WriteLine("----------------------------");
                            
                            int catId;
                            while (true)
                            {
                                catId = GetValidInt("Enter Category ID: ");
                                if (inventory.GetCategories().Any(c => c.Id == catId)) break;
                                Console.WriteLine("[Error] Category not found. Please try again.");
                            }
                            
                            Console.WriteLine("\n--- Available Suppliers ---");
                            foreach (var sup in inventory.GetSuppliers())
                            {
                                Console.WriteLine($"  [ID: {sup.Id, -2}] {sup.Name}");
                            }
                            Console.WriteLine("---------------------------");
                            
                            int supId;
                            while (true)
                            {
                                supId = GetValidInt("Enter Supplier ID: ");
                                if (inventory.GetSuppliers().Any(s => s.Id == supId)) break;
                                Console.WriteLine("[Error] Supplier not found. Please try again.");
                            }
                            decimal price = GetValidDecimal("Enter Price (Php): ");
                            int stock = GetValidInt("Enter Initial Stock Quantity: ");
                            int threshold = GetValidInt("Enter Low Stock Threshold: ");
                            
                            inventory.AddProduct(prodName, catId, supId, price, stock, threshold);
                            break;

                        case "4":
                            PrintHeader("ALL PRODUCTS");
                            var allProducts = inventory.GetProducts();
                            if (!allProducts.Any())
                            {
                                Console.WriteLine("No products available in the inventory.");
                                break;
                            }

                            foreach (var cat in inventory.GetCategories())
                            {
                                var categoryProducts = allProducts.Where(p => p.CategoryId == cat.Id).ToList();
                                if (!categoryProducts.Any()) continue; // Skip categories with no products

                                Console.WriteLine($"\n[{cat.Name.ToUpper()}]");

                                var supplierIds = categoryProducts.Select(p => p.SupplierId).Distinct();
                                foreach (var sId in supplierIds)
                                {
                                    var supplier = inventory.GetSuppliers().FirstOrDefault(s => s.Id == sId);
                                    var supplierName = supplier?.Name ?? "Unknown Supplier";
                                    var supplierContact = supplier?.ContactInfo ?? "N/A";
                                    Console.WriteLine($"  Supplier: {supplierName} (Contact: {supplierContact})");

                                    foreach (var prod in categoryProducts.Where(p => p.SupplierId == sId))
                                    {
                                        Console.WriteLine($"    -> [ID: {prod.Id, -2}] {prod.Name,-18} | Price: Php {prod.Price,-8} | Stock: {prod.StockQuantity}");
                                    }
                                }
                            }
                            break;

                        case "5":
                            Console.Write("Enter search keyword (type 'cancel' to abort, or press Enter for all): ");
                            string keyword = Console.ReadLine() ?? "";
                            if (keyword.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase)) throw new OperationCanceledException();
                            
                            PrintHeader($"SEARCH RESULTS FOR '{keyword}'");
                            
                            var searchResults = inventory.SearchProducts(keyword);
                            if (!searchResults.Any())
                            {
                                Console.WriteLine("No products found matching your keyword.");
                                break;
                            }

                            foreach (var prod in searchResults)
                            {
                                Console.WriteLine($"Found: [ID: {prod.Id}] {prod.Name,-18} | Price: Php {prod.Price}");
                            }

                            Console.WriteLine();
                            int detailId;
                            while (true)
                            {
                                detailId = GetValidInt("Enter Product ID to view details (or 0 to cancel): ");
                                if (detailId == 0) break;
                                if (searchResults.Any(p => p.Id == detailId)) break;
                                Console.WriteLine("[Error] Product ID not found in the search results. Please try again.");
                            }

                            if (detailId == 0) break;

                            var selectedProduct = searchResults.First(p => p.Id == detailId);
                            var prodCategory = inventory.GetCategories().FirstOrDefault(c => c.Id == selectedProduct.CategoryId);
                            var prodSupplier = inventory.GetSuppliers().FirstOrDefault(s => s.Id == selectedProduct.SupplierId);

                            PrintHeader("PRODUCT DETAILS");
                            Console.WriteLine($"ID:                  {selectedProduct.Id}");
                            Console.WriteLine($"Name:                {selectedProduct.Name}");
                            Console.WriteLine($"Category:            {prodCategory?.Name ?? "Unknown"}");
                            Console.WriteLine($"Supplier:            {prodSupplier?.Name ?? "Unknown"}");
                            Console.WriteLine($"Price:               Php {selectedProduct.Price}");
                            Console.WriteLine($"Stock Quantity:      {selectedProduct.StockQuantity}");
                            Console.WriteLine($"Low Stock Threshold: {selectedProduct.LowStockThreshold}");
                            break;

                        case "6":
                            if (!inventory.GetProducts().Any())
                            {
                                Console.WriteLine("[Error] No products available to update.");
                                break;
                            }

                            Console.WriteLine("\n--- Select Category ---");
                            foreach (var cat in inventory.GetCategories()) Console.WriteLine($"  [ID: {cat.Id, -2}] {cat.Name}");
                            int catIdUpdate = GetValidInt("Enter Category ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Select Supplier ---");
                            foreach (var sup in inventory.GetSuppliers()) Console.WriteLine($"  [ID: {sup.Id, -2}] {sup.Name}");
                            int supIdUpdate = GetValidInt("Enter Supplier ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Available Products ---");
                            var productsToUpdate = inventory.GetProducts().Where(p => p.CategoryId == catIdUpdate && p.SupplierId == supIdUpdate).ToList();
                            if (!productsToUpdate.Any())
                            {
                                Console.WriteLine("No products found for this category and supplier.");
                                break;
                            }
                            foreach (var prod in productsToUpdate) Console.WriteLine($"  [ID: {prod.Id, -2}] {prod.Name} (Php {prod.Price})");
                            Console.WriteLine("--------------------------");

                            int updateId;
                            while (true)
                            {
                                updateId = GetValidInt("Enter Product ID to Update: ");
                                if (productsToUpdate.Any(p => p.Id == updateId)) break;
                                Console.WriteLine("[Error] Product ID not found in the list above. Please try again.");
                            }

                            var existingProd = inventory.GetProducts().First(p => p.Id == updateId);

                            string newName = existingProd.Name;
                            decimal newPrice = existingProd.Price;
                            int newThreshold = existingProd.LowStockThreshold;

                            Console.WriteLine("\nWhat do you want to update?");
                            Console.WriteLine("1. Name");
                            Console.WriteLine("2. Price");
                            Console.WriteLine("3. Low Stock Threshold");
                            Console.WriteLine("4. Update All");
                            string updateChoice = GetValidString("Enter choice: ");

                            if (updateChoice == "1" || updateChoice == "4") {
                                newName = GetValidString($"Enter New Name (Current: {existingProd.Name}): ");
                            }
                            if (updateChoice == "2" || updateChoice == "4") {
                                newPrice = GetValidDecimal($"Enter New Price (Php) (Current: {existingProd.Price}): ");
                            }
                            if (updateChoice == "3" || updateChoice == "4") {
                                newThreshold = GetValidInt($"Enter New Low Stock Threshold (Current: {existingProd.LowStockThreshold}): ");
                            }
                            
                            inventory.UpdateProduct(updateId, newName, newPrice, newThreshold);
                            break;

                        case "7":
                            if (!inventory.GetProducts().Any())
                            {
                                Console.WriteLine("[Error] No products available to delete.");
                                break;
                            }

                            Console.WriteLine("\n--- Select Category ---");
                            foreach (var cat in inventory.GetCategories()) Console.WriteLine($"  [ID: {cat.Id, -2}] {cat.Name}");
                            int catIdDelete = GetValidInt("Enter Category ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Select Supplier ---");
                            foreach (var sup in inventory.GetSuppliers()) Console.WriteLine($"  [ID: {sup.Id, -2}] {sup.Name}");
                            int supIdDelete = GetValidInt("Enter Supplier ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Available Products ---");
                            var productsToDelete = inventory.GetProducts().Where(p => p.CategoryId == catIdDelete && p.SupplierId == supIdDelete).ToList();
                            if (!productsToDelete.Any())
                            {
                                Console.WriteLine("No products found for this category and supplier.");
                                break;
                            }
                            foreach (var prod in productsToDelete) Console.WriteLine($"  [ID: {prod.Id, -2}] {prod.Name} (Php {prod.Price})");
                            Console.WriteLine("--------------------------");

                            int deleteId;
                            while (true)
                            {
                                deleteId = GetValidInt("Enter Product ID to Delete: ");
                                if (productsToDelete.Any(p => p.Id == deleteId)) break;
                                Console.WriteLine("[Error] Product ID not found in the list above. Please try again.");
                            }
                            
                            inventory.DeleteProduct(deleteId);
                            break;

                        case "8":
                            if (!inventory.GetProducts().Any())
                            {
                                Console.WriteLine("[Error] No products available to adjust stock.");
                                break;
                            }

                            Console.WriteLine("\n--- Select Category ---");
                            foreach (var cat in inventory.GetCategories()) Console.WriteLine($"  [ID: {cat.Id, -2}] {cat.Name}");
                            int catIdStock = GetValidInt("Enter Category ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Select Supplier ---");
                            foreach (var sup in inventory.GetSuppliers()) Console.WriteLine($"  [ID: {sup.Id, -2}] {sup.Name}");
                            int supIdStock = GetValidInt("Enter Supplier ID (type 'cancel' to abort): ");

                            Console.WriteLine("\n--- Available Products ---");
                            var productsToStock = inventory.GetProducts().Where(p => p.CategoryId == catIdStock && p.SupplierId == supIdStock).ToList();
                            if (!productsToStock.Any())
                            {
                                Console.WriteLine("No products found for this category and supplier.");
                                break;
                            }
                            foreach (var prod in productsToStock) Console.WriteLine($"  [ID: {prod.Id, -2}] {prod.Name} (Current Stock: {prod.StockQuantity})");
                            Console.WriteLine("--------------------------");

                            int stockProdId;
                            while (true)
                            {
                                stockProdId = GetValidInt("Enter Product ID to adjust stock: ");
                                if (productsToStock.Any(p => p.Id == stockProdId)) break;
                                Console.WriteLine("[Error] Product ID not found in the list above. Please try again.");
                            }

                            string action;
                            while (true)
                            {
                                Console.WriteLine("\nChoose Action:");
                                Console.WriteLine("1. Restock (Add Stock)");
                                Console.WriteLine("2. Deduct (Remove Stock)");
                                action = GetValidString("Enter choice (1 or 2, type 'cancel' to abort): ");
                                if (action == "1" || action == "2") break;
                                Console.WriteLine("[Error] Invalid action choice. Please try again.");
                            }

                            Console.WriteLine(action == "1" ? "\n--- Restock ---" : "\n--- Deduct ---");

                            while (true)
                            {
                                try
                                {
                                    int amount = GetValidInt("Enter amount (type 'cancel' to abort): ");
                                    
                                    if (action == "1") inventory.RestockProduct(stockProdId, amount);
                                    else inventory.DeductProduct(stockProdId, amount);
                                    
                                    break; // Success, exit the loop
                                }
                                catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
                                {
                                    Console.WriteLine($"[Error] {ex.Message} Please try again.");
                                }
                            }
                            
                            break;

                        case "9":
                            bool reportMenu = true;
                            while (reportMenu)
                            {
                                Console.Clear();
                                TopBorder();
                                CenteredRow("INVENTORY REPORTS");
                                SectionDivider();
                                MenuRow("1", "Transaction History");
                                MenuRow("2", "Low Stock Items");
                                MenuRow("3", "Total Inventory Value");
                                MenuRow("0", "Back to Main Menu");
                                BottomBorder();
                                Console.Write("\n  Enter choice: ");

                                string repChoice = Console.ReadLine() ?? "";
                                if (repChoice == "0") break;

                                if (repChoice == "1")
                                {
                                    PrintHeader("TRANSACTION HISTORY");
                                    var trans = inventory.GetTransactions();
                                    if (!trans.Any()) Console.WriteLine("No transactions recorded yet.");
                                    else
                                    {
                                        foreach (var t in trans)
                                        {
                                            var pName = inventory.GetProducts().FirstOrDefault(p => p.Id == t.ProductId)?.Name ?? "Unknown";
                                            Console.WriteLine($"[ID: {t.TransactionId}] {t.Date:yyyy-MM-dd HH:mm} | {t.TransactionType,-7} | {t.Quantity,4} units | Product: {pName,-15} | By: {t.PerformedBy} ({t.Role})");
                                        }
                                    }
                                }
                                else if (repChoice == "2")
                                {
                                    PrintHeader("LOW STOCK ITEMS");
                                    var lowStock = inventory.GetLowStockProducts();
                                    if (!lowStock.Any()) Console.WriteLine("All products are sufficiently stocked.");
                                    else
                                    {
                                        foreach (var p in lowStock) Console.WriteLine($"[ID: {p.Id}] {p.Name,-18} | Stock: {p.StockQuantity} (Threshold: {p.LowStockThreshold})");
                                    }
                                }
                                else if (repChoice == "3")
                                {
                                    PrintHeader("TOTAL INVENTORY VALUE");
                                    decimal totalVal = inventory.GetTotalInventoryValue();
                                    Console.WriteLine($"Total Value of all stock: Php {totalVal:N2}");
                                }
                                else Console.WriteLine("[Error] Invalid choice.");

                                Console.WriteLine("\nPress Enter to continue...");
                                Console.ReadLine();
                            }
                            break;

                        case "0":
                            isRunning = false;
                            Console.Clear();
                            Console.WriteLine();
                            TopBorder();
                            CenteredRow("GOODBYE!");
                            SectionDivider();
                            EmptyRow();
                            CenteredRow("Thank you for using IMS.");
                            EmptyRow();
                            BottomBorder();
                            Console.WriteLine();
                            System.Threading.Thread.Sleep(1200);
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\n[Info] Operation canceled. Returning to main menu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void PrintHeader(string title)
        {
            Console.WriteLine();
            TopBorder();
            CenteredRow(title.ToUpper());
            BottomBorder();
            Console.WriteLine();
        }

        static string GetValidString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (input.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    throw new OperationCanceledException();
                
                if (!string.IsNullOrWhiteSpace(input)) return input;
                Console.WriteLine("[Error] Input cannot be empty. Please try again.");
            }
        }

        static int GetValidInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (input.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    throw new OperationCanceledException();
                
                if (int.TryParse(input, out result) && result >= 0) return result;
                Console.WriteLine("[Error] Invalid input. Please enter a valid positive number.");
            }
        }

        static decimal GetValidDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";
                if (input.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    throw new OperationCanceledException();
                
                if (decimal.TryParse(input, out result) && result >= 0) return result;
                Console.WriteLine("[Error] Invalid input. Please enter a valid positive number.");
            }
        }

        // ── UI Helper Methods ─────────────────────────────────────────
        static void TopBorder() => Console.WriteLine("+" + new string('=', WIDTH) + "+");
        static void BottomBorder() => Console.WriteLine("+" + new string('=', WIDTH) + "+");
        static void SectionDivider() => Console.WriteLine("+" + new string('=', WIDTH) + "+");
        static void ThinDivider() => Console.WriteLine("|" + new string('-', WIDTH) + "|");
        static void EmptyRow() => Console.WriteLine("|" + new string(' ', WIDTH) + "|");

        static void CenteredRow(string text)
        {
            if (text.Length > WIDTH) text = text.Substring(0, WIDTH);
            int totalPad = WIDTH - text.Length;
            int left = totalPad / 2;
            int right = totalPad - left;
            Console.WriteLine("|" + new string(' ', left) + text + new string(' ', right) + "|");
        }

        static void MenuRow(string num, string label)
        {
            string content = $"  [ {num} ]  {label}";
            string padded = content.PadRight(WIDTH);
            if (padded.Length > WIDTH) padded = padded.Substring(0, WIDTH);
            Console.WriteLine("|" + padded + "|");
        }
    }
}
