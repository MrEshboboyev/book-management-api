# 📚 Book Management API – Scalable & Secure REST API 🚀  

![.NET 9](https://img.shields.io/badge/.NET%209-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-%F0%9F%93%9A-red?style=for-the-badge)
![CQRS](https://img.shields.io/badge/CQRS-%E2%9C%85-green?style=for-the-badge)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-%F0%9F%9A%80-purple?style=for-the-badge)
![JWT Authentication](https://img.shields.io/badge/JWT%20Authentication-%F0%9F%94%92-yellow?style=for-the-badge)

## 🎯 Overview  

This **Book Management API** is a **high-performance, scalable REST API** built with **ASP.NET Core** and **C#**, implementing **Clean Architecture, Domain-Driven Design (DDD), and CQRS**. It supports **CRUD operations, soft deletion, pagination, JWT authentication, popularity score calculation**, and **logging with validation pipelines**.  

**Key Features:**  
✅ **Secure** – JWT-based authentication with role-based authorization.  
✅ **Efficient** – Implements **pagination, caching, and retry mechanisms**.  
✅ **Scalable** – Built with **CQRS, Event-Driven Architecture, and Outbox Pattern**.  
✅ **Reliable** – Uses **Quartz for background jobs** and **Polly for resiliency**.  

---

## 🏛️ Architecture & Design Patterns  

This project follows **Clean Architecture** with **Domain-Driven Design (DDD)** principles:  

📌 **Domain Layer** – Entities, Value Objects, Aggregates, and Domain Events.  
📌 **Application Layer** – Commands, Queries, Services, and Event Handlers.  
📌 **Persistence Layer** – EF Core, SQL Server, Interceptors, Repository implementations.  
📌 **Infrastructure Layer** – Quartz Background Jobs, JWT, Idempotence.  
📌 **Presentation Layer** – ASP.NET Core Web API with Swagger Documentation.  

### **🔹 Key Patterns & Technologies Used**  
🔹 **CQRS** – Segregates commands (writes) and queries (reads).  
🔹 **Outbox Pattern** – Ensures reliable event-driven architecture.  
🔹 **Rich Domain Model** – Encapsulates behavior within entities.  
🔹 **Retry Mechanism (Polly)** – Ensures reliability for external calls.  
🔹 **Idempotence** – Prevents duplicate API requests.  
🔹 **Logging & Validation Pipelines** – Ensures clean, validated, and logged API requests.  
🔹 **Global Exception Handling Middleware** – Centralized error handling.  

---

## 🛠️ Technologies & Tools  

- **Framework** – ASP.NET Core 8 / 9 Web API  
- **Database** – SQL Server, EF Core  
- **Caching & Resiliency** – Polly for retry mechanism  
- **Background Jobs** – Quartz.NET  
- **Security** – JWT-based authentication and permission-based authorization  
- **API Documentation** – Swagger / OpenAPI  
- **Logging & Monitoring** – Serilog  
- **Validation** – FluentValidation  
- **Dependency Injection** – Built-in .NET DI container  

---

## 📖 Functional Features  

### **1️⃣ CRUD Operations**  
✅ **Add Books** – Supports **single & bulk inserts**.  
✅ **Update Books** – Modify book details with proper validation.  
✅ **Soft Delete Books** – Books can be restored if needed.  
✅ **Retrieve Books** – Fetch books **by popularity** (most-viewed first) with pagination.  
✅ **Get Book Details** – Includes **real-time popularity score calculation**.  

### **2️⃣ Popularity Score Calculation**  
📌 **Formula:**  
```
Popularity Score = (BookViews * 0.5) + (YearsSincePublished * 2)
```
📌 **How it works:**  
- **BookViews** – Counts the number of times a book’s details are retrieved.  
- **YearsSincePublished** – Older books receive a smaller score boost.  
- **Live Computation** – Popularity is calculated **on the fly** (not stored in DB).  

### **3️⃣ Security & Authentication**  
✅ **JWT-based authentication** – Secures all API endpoints.  
✅ **Role-based authorization** – Restricts access based on user permissions.  

### **4️⃣ Pagination & Filtering**  
✅ **Retrieve books in order of popularity** with pagination support.  

---

## 🚀 Getting Started  

### **📌 Prerequisites**  
✅ [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
✅ [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
✅ [Docker](https://www.docker.com/) (optional for running SQL Server container)  

### **Step 1: Clone the Repository**  
```bash
git clone https://github.com/MrEshboboyev/book-management-api.git
cd book-management-api
```

### **Step 2: Configure Database**  
Set up **SQL Server** and update connection strings in `appsettings.json`:  
```json
"ConnectionStrings": {
  "SqlServerDatabase": "Server=localhost;Database=BookManagement_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

### **Step 3: Run Database Migrations**  
```bash
dotnet ef database update
```

### **Step 4: Run the Application**  
```bash
dotnet run --project src/BookManagement.App
```

---

## 🔗 API Endpoints  

Auth: 
| Method | Endpoint             | Description |
|--------|---------------------|-------------|
| **POST** | `/api/auth/login`       | Logs in a user by validating their credentials and generating a token. |
| **POST**  | `/api/auth/register`  | Registers a new user by creating their account with the provided details. |

Users: 
| Method | Endpoint             | Description |
|--------|---------------------|-------------|
| **GET**  | `/api/users`  | Retrieves the details of a current user. |

Books: 
| Method | Endpoint             | Description |
|--------|---------------------|-------------|
| **POST** | `/api/books`       | Adds a new book. |
| **POST** | `/api/books/bulk`       | Adds multiple books in bulk. |
| **PUT**  | `/api/books/{id:guid}`  | Updates an existing book. |
| **DELETE** | `/api/books/{id:guid}` | Soft delete a book |
| **DELETE** | `/api/books/bulk` | Soft deletes multiple books in bulk |
| **GET**  | `/api/books`       | Retrieve books by popularity (paginated) |
| **GET**  | `/api/books/{id}`  | Retrieves the details of a book by its unique identifier & popularity score |

---

## 🧪 Testing  

### **Unit Tests**  
Run unit tests for validation, application layer, and controllers:  
```bash
dotnet test
```

### **Manual API Testing**  
📌 **Use Postman** or any REST client to:  
✅ **Register/Login** → Obtain a JWT Token  
✅ **Add Books** → `/api/books`  
✅ **Retrieve Books** → `/api/books?PageNumber=1&PageSize=10`  
✅ **Get Book Details** → `/api/books/{id}` (track popularity score)  

---

## 🎯 Why Use This Project?  

✅ **Enterprise-Grade Architecture** – Clean, scalable, and maintainable.  
✅ **Performance-Oriented** – Optimized with CQRS, caching, and indexing.  
✅ **Security-First Approach** – Uses JWT authentication and role-based access.  
✅ **Ready for Production** – Implements industry best practices.  

---

## 📜 License  

This project is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.  

---

## 📞 Contact  

For feedback, contributions, or questions:  
📧 **Email**: mreshboboyev@gmail.com

💻 **GitHub**: [MrEshboboyev](https://github.com/MrEshboboyev)  

---

🚀 **Build high-performance, scalable APIs with .NET!** Clone the repo & start coding today!  
