# Grocery Store Web Application

A full-stack web application for managing a grocery store, built with **ASP.NET Core** and **Entity Framework Core**. The project supports user authentication, product management, shopping cart, order processing, and reviews.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Features](#features)
- [Setup & Installation](#setup--installation)
- [Configuration](#configuration)
- [Database](#database)
- [API Endpoints](#api-endpoints)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)

---

## Project Structure

```
.
├── Data/                  # Data access layer, models, DTOs, interfaces
│   ├── DataAccess/        # Data access logic for each domain
│   ├── Dto/               # Data Transfer Objects
│   ├── Interfaces/        # Service interfaces
│   └── Models/            # Entity models and DbContext
├── GroceryStore/          # Main ASP.NET Core web application
│   ├── Controllers/       # API controllers
│   ├── Pages/             # Razor Pages/views
│   ├── wwwroot/           # Static files (uploads, favicon, etc.)
│   ├── Program.cs         # Application entry point
│   ├── Startup.cs         # App configuration and middleware
│   ├── appsettings.json   # App configuration
│   └── Properties/        # Launch settings
├── GroceryStore.sln       # Solution file
└── README.md              # Project documentation
```

---

## Features

- **User Authentication** (JWT-based)
- **Admin & User Roles** with policy-based authorization
- **Product Management** (CRUD for groceries, categories)
- **Shopping Cart** and **Order Processing**
- **Product Reviews**
- **RESTful API** with Swagger documentation
- **File Uploads** for product images
- **Entity Framework Core** for database access

---

## Setup & Installation

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd <repo-root>
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Update Database Connection:**
   - Edit `GroceryStore/appsettings.json` and set your SQL Server connection string under `ConnectionStrings:DefaultConnection`.

4. **Apply Migrations (if needed):**
   ```bash
   dotnet ef database update --project Data
   ```

5. **Run the application:**
   ```bash
   dotnet run --project GroceryStore
   ```

6. **Access the app:**
   - By default, the app runs at `https://localhost:5001` or `http://localhost:5000`.

---

## Configuration

The main configuration file is `GroceryStore/appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-secret-key",
    "Issuer": "https://localhost:44364/",
    "Audience": "http://localhost:4200/"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-connection-string"
  },
  "DefaultCoverImageFile": "Default_image.jpg",
  "AllowedHosts": "*"
}
```

- **Jwt**: Used for authentication. Change the `SecretKey` for production.
- **ConnectionStrings**: Update with your SQL Server details.

---

## Database

- Uses **Entity Framework Core**.
- Main context: `GroceryDBContext` (see `Data/Models/GroceryDBContext.cs`)
- Entities: `Grocery`, `Cart`, `CartItems`, `Categories`, `CustomerOrderDetails`, `CustomerOrders`, `UserMaster`, `UserType`, `Review`, etc.

---

## API Endpoints

The API is organized by controllers in `GroceryStore/Controllers/`:

- **GroceryController**: `/api/grocery`
  - `GET /api/grocery` — List all groceries
  - `GET /api/grocery/{id}` — Get grocery by ID
  - `POST /api/grocery` — Add new grocery (Admin only)
  - `PUT /api/grocery` — Update grocery (Admin only)
  - `DELETE /api/grocery/{id}` — Delete grocery (Admin only)
  - `GET /api/grocery/GetCategoriesList` — List categories

- **ShoppingCartController**: `/api/shoppingcart`
- **OrderController**: `/api/order`
- **UserController**: `/api/user`
- **ReviewController**: `/api/review`
- **LoginController**: `/api/login`
- **CheckOutController**: `/api/checkout`

> **Full API documentation is available via Swagger at `/swagger` when running the app.**

---

## Development

- **.NET Core SDK 3.1+** required.
- Uses **JWT authentication** and **policy-based authorization**.
- **Swagger** is enabled for API documentation.
- Static files (e.g., product images) are stored in `GroceryStore/wwwroot/Upload/`.

---

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

---

## License

This project is licensed under the MIT License.

---

**Author:** Ibrahim Saleem  
**Contact:** [LinkedIn](https://www.linkedin.com/in/ibrahimsaleem91/)