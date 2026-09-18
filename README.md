# CustomerProduct API

A backend REST API built with ASP.NET Core for managing customers and their products, built as a learning project to practice backend development, Entity Framework Core, and asynchronous messaging with RabbitMQ.

## Features

- CRUD operations for Customers and Products
- One-to-many relationship between Customer and Product (via EF Core)
- DTO-based request/response models with validation
- Database migrations with Entity Framework Core
- Asynchronous event publishing via RabbitMQ on product creation
- Swagger/OpenAPI documentation

## Tech Stack

- ASP.NET Core (.NET 8)
- Entity Framework Core
- SQL Server
- RabbitMQ

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote instance)
- A RabbitMQ instance (e.g. [CloudAMQP](https://www.cloudamqp.com/))

### Setup

1. Clone the repository

git clone <repo-url>


2. Update the connection string in `appsettings.json` with your own SQL Server instance.

3. Set your RabbitMQ credentials using [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

dotnet user-secrets set "RabbitMQ:HostName" "your-host"
dotnet user-secrets set "RabbitMQ:UserName" "your-username"
dotnet user-secrets set "RabbitMQ:Password" "your-password"
dotnet user-secrets set "RabbitMQ:VirtualHost" "your-vhost"
dotnet user-secrets set "RabbitMQ:Port" "5672"


4. Apply database migrations:

Update-Database


5. Run the project — Swagger UI will open automatically at `/swagger`.

## API Endpoints

| Method | Endpoint            | Description             |
|--------|----------------------|--------------------------|
| GET    | `/api/customer`      | List all customers      |
| GET    | `/api/customer/{id}` | Get a customer by id     |
| POST   | `/api/customer`      | Create a new customer   |
| PUT    | `/api/customer/{id}` | Update a customer       |
| DELETE | `/api/customer/{id}` | Delete a customer       |
| GET    | `/api/product`       | List products (optional `?customerId=`) |
| GET    | `/api/product/{id}`  | Get a product by id      |
| POST   | `/api/product`       | Create a new product (publishes a RabbitMQ event) |
| PUT    | `/api/product/{id}`  | Update a product        |
| DELETE | `/api/product/{id}`  | Delete a product        |

## Notes

This is a learning/prototype project — built primarily to practice ASP.NET Core fundamentals (EF Core, DTOs, validation, migrations) and explore asynchronous messaging patterns with RabbitMQ. The RabbitMQ integration currently only publishes events; no consumer has been implemented yet.
