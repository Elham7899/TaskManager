# 🚀 Task Manager API - Enterprise-Grade Task Management System

[![ASP.NET Core 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)](https://www.postgresql.org)
[![Docker](https://img.shields.io/badge/Docker-2.0-2496ED?logo=docker)](https://www.docker.com)
[![Swagger](https://img.shields.io/badge/Swagger-3.0-85EA2D?logo=swagger)](https://swagger.io)

> Production-ready task management API with enterprise security, containerization, and clean architecture implementation

![Architecture Diagram](https://via.placeholder.com/800x400.png?text=Clean+Architecture+Diagram)
*(Pro Tip: Add architecture diagram with [draw.io](https://app.diagrams.net/))*

## 🌟 Core Features

### 🔒 Security & Authentication
- JWT Bearer Authentication with refresh tokens
- Role-based Authorization (RBAC)
- Password hashing with PBKDF2
- Token revocation mechanism

### 📦 Domain-Centric Architecture
| Layer          | Responsibilities                          |
|----------------|-------------------------------------------|
| **Domain**     | Entities, Enums, Interfaces, Exceptions   |
| **Application**| CQRS Handlers, DTOs, Validators, Services |
| **Infrastructure**| EF Core, PostgreSQL, Identity, Caching  |
| **API**        | Controllers, Middleware, Swagger config   |

### ⚙️ Operational Excellence
- Dockerized PostgreSQL with persistent volume
- Health Check endpoints (`/health`)
- Request/Response logging with Serilog
- Automated database migrations on startup
- Swagger UI with JWT support

## 📚 API Endpoints
Method	Endpoint	Description	Auth Required
POST	/api/auth/login	Get JWT token	❌
POST	/api/tasks	Create new task	✅
GET	/api/tasks?status=InProgress	Filter tasks	✅
PUT	/api/tasks/{id}	Update task	✅

## 🧪 Testing Strategy
graph TD
    A[Unit Tests] --> B[Domain Layer]
    A --> C[Application Layer]
    D[Integration Tests] --> E[API Endpoints]
    D --> F[Database Interactions]
    G[Postman] --> H[E2E Workflow Validation]
    
Run tests with:
    dotnet test

## 🔧 Environment Configuration
Configure via appsettings.Development.json:
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
Implement SignalR real-time updates
Integrate Azure Key Vault for secrets
Add OpenTelemetry instrumentation
Develop React frontend (separate repo)
Implement CI/CD pipeline (GitHub Actions)

## 🤝 Contribution Guidelines
Create feature branches from develop branch
Follow CQRS pattern for new features
Maintain 80%+ test coverage
Document API changes in Swagger
Update docker-compose for new services

# Example workflow:
git checkout -b feature/auth-enhancements
# Make changes
dotnet format # Enforce code style
dotnet test   # Verify tests
git push origin feature/auth-enhancements

## 📜 License
MIT License - See LICENSE for details

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker Desktop
- PostgreSQL 16

### Local Development
```bash
# Clone repository
git clone https://github.com/Elham7899/TaskManager.git

# Start database container
docker compose up -d postgres

# Run application
dotnet run --project src/API
