# CLI Inventory Management System

A robust, CLI-based Inventory Management System built in C# using strict Object-Oriented Programming (OOP) principles.

## Features
This application features a fully interactive, box-styled command-line interface with the following capabilities:
- **User Management**: Simple startup login system with role tracking.
- **Core Inventory**: Add, view, search, update, and delete Categories, Suppliers, and Products.
- **Stock Adjustments**: Safely restock or deduct product quantities with built-in validation.
- **Inventory Reports**: 
  - View detailed Transaction History (tracks date, action, quantity, and the user/role who performed it).
  - View Low Stock Items based on custom thresholds.
  - Calculate the Total Inventory Value dynamically.

## Technical Highlights
- **OOP Principles**: Utilizes constructors, properties with `private set`, encapsulation, and restricted access modifiers.
- **In-Memory Storage**: Uses generic `List<T>` for fast, runtime data management (No external database).
- **Robust Error Handling**: Implements `try-catch` blocks, custom exceptions (like `OperationCanceledException`), and infinite retry loops to prevent application crashes and ensure smooth user experience.

## Application Structure (Models)
The system relies on 5 core OOP models interacting with one another:
1. `Product`
2. `Category`
3. `Supplier`
4. `User`
5. `TransactionRecord`

## How to Run
Ensure you have the .NET SDK installed on your machine.

1. Clone the repository or download the source code.
2. Open your terminal or command prompt.
3. Navigate to the project folder where `Program.cs` is located.
4. Run the following command:
   ```bash
   dotnet run
   ```
5. Enter a username and role to log in, and explore the main menu!