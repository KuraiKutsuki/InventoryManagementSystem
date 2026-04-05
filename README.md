<div align="center">

# 📦 CLI Inventory Management System

A robust, CLI-based Inventory Management System built in **C#** using strict **Object-Oriented Programming (OOP)** principles.

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-blue?style=for-the-badge)
![Status](https://img.shields.io/badge/status-active-success?style=for-the-badge)

</div>

---

## ✨ Features

This application features a fully interactive, **box-styled** command-line interface with the following capabilities:

| Feature | Description |
|---|---|
| 👤 **User Management** | Simple startup login system with role tracking |
| 🗃️ **Core Inventory** | Add, view, search, update, and delete Categories, Suppliers, and Products |
| 📊 **Stock Adjustments** | Safely restock or deduct product quantities with built-in validation |
| 📋 **Inventory Reports** | Transaction history, low stock alerts, and total value calculation |

### 📋 Inventory Reports (Detail)
- 🕓 View detailed **Transaction History** — tracks date, action, quantity, and the user/role who performed it
- ⚠️ View **Low Stock Items** based on custom thresholds
- 💰 Calculate the **Total Inventory Value** dynamically

---

## ⚙️ Technical Highlights

- 🧱 **OOP Principles** — Utilizes constructors, properties with `private set`, encapsulation, and restricted access modifiers
- 🧠 **In-Memory Storage** — Uses generic `List<T>` for fast, runtime data management *(No external database)*
- 🛡️ **Robust Error Handling** — Implements `try-catch` blocks, custom exceptions (like `OperationCanceledException`), and infinite retry loops to prevent crashes and ensure a smooth user experience

---

## 🏗️ Application Structure

The system relies on **5 core OOP models** interacting with one another:

```
InventoryManagementSystem/
├── 📦 Product
├── 🏷️ Category
├── 🚚 Supplier
├── 👤 User
└── 📝 TransactionRecord
```

---

## 🚀 How to Run

> **Prerequisites:** Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

**1.** Clone the repository or download the source code:
```bash
git clone https://github.com/KuraiKutsuki/InventoryManagementSystem.git
```

**2.** Navigate to the project folder:
```bash
cd InventoryManagementSystem
```

**3.** Run the application:
```bash
dotnet run
```

**4.** Enter a **username** and **role** to log in, and explore the main menu! 🎉

---

<div align="center">

Made with ❤️ by [KuraiKutsuki](https://github.com/KuraiKutsuki)

</div>
