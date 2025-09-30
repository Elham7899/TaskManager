# 🚀 Task Manager API - Enterprise-Grade Task Management System

[![ASP.NET Core 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)](https://www.postgresql.org)
[![Docker](https://img.shields.io/badge/Docker-2.0-2496ED?logo=docker)](https://www.docker.com)
[![Swagger](https://img.shields.io/badge/Swagger-3.0-85EA2D?logo=swagger)](https://swagger.io)

> Production-ready task management API with enterprise security, containerization, and clean architecture implementation

## 🌟 Core Features
## 🔒 Security & Authentication

JWT Bearer Authentication with refresh tokens
Role-based Authorization (RBAC)
Secure password hashing with PBKDF2
Token revocation mechanism

## 📦 Domain-Centric Architecture (CQRS + MediatR)
| Layer              | Responsibilities                                                   |
| ------------------ | ------------------------------------------------------------------ |
| **Domain**         | Entities, Enums, Exceptions                                        |
| **Application**    | CQRS Handlers, DTOs, Validators, Mapping, Pipeline Behaviors       |
| **Infrastructure** | EF Core (PostgreSQL/InMemory), Identity, Persistence, Caching      |
| **API**            | Controllers, Middleware, Swagger, Auth, Centralized Error Handling |

✅ CQRS for tasks and labels
✅ FluentValidation + pipeline behaviors for validation & error handling
✅ Pagination support via PagedResult<T> & PaginationMetadata
✅ Consistent API responses via ApiResponse<T>
✅ Unit tests for handlers (no service layer dependencies)

## ⚙️ Operational Excellence

Dockerized PostgreSQL with persistent volumes
Automated EF Core migrations on startup
Health check endpoints (/health)
Request/Response logging via Serilog
Swagger UI with JWT authentication support

## 📚 API Endpoints
| Method | Endpoint                       | Description     | Auth Required |
| ------ | ------------------------------ | --------------- | ------------- |
| POST   | `/api/auth/login`              | Get JWT token   | ❌             |
| POST   | `/api/tasks`                   | Create new task | ✅             |
| GET    | `/api/tasks?status=InProgress` | Filter tasks    | ✅             |
| PUT    | `/api/tasks/{id}`              | Update task     | ✅             |



## 🧪 Testing Strategy

All CQRS handlers (Create, Update, Delete, Assign, Remove, Query) are covered with unit tests using:
InMemoryDatabase for isolation
FluentAssertions for readable assertions
Proper setups for CreatedBy / UpdatedBy and relationships (TaskLabel)
Run tests with:
dotnet test

## 🔧 Environment Configuration

appsettings.Development.json:
{
  "Jwt": {
    "Secret": "your-256-bit-secret",
    "ExpiryHours": 72
  },
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Database=taskdb;Username=postgres;Password=mysecretpassword"
  }
}

## 🛠 Future Roadmap
 Add Redis caching layer
 Real-time updates with SignalR
 Secure secrets via Azure Key Vault
 Distributed tracing with OpenTelemetry
 CI/CD pipeline (GitHub Actions)
 Analytics + Swagger improvements
 Full React frontend integration (separate repo)

## 🤝 Contribution Guidelines
Create feature branches from develop
Follow CQRS pattern for new features
Maintain 80%+ test coverage
Document all API changes in Swagger
Update docker-compose for new services

# Example workflow:
git checkout -b feature/auth-enhancements
# Make changes
dotnet format      # Enforce code style
dotnet test        # Run unit tests
git push origin feature/auth-enhancements

## 📜 License
MIT License — see LICENSE
 for details.

## 🚀 Getting Started
### Prerequisites
.NET 8 SDK
Docker Desktop
PostgreSQL 16
Local Development

### Local Development
# Clone repository
git clone https://github.com/Elham7899/TaskManager.git

# Start PostgreSQL container
docker compose up -d postgres

# Run the API
dotnet run --project src/API

