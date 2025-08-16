# Technology Stack & Build System

## Architecture Pattern
- **Backend**: Clean Architecture with CQRS and Domain-Driven Design (DDD)
- **Frontend**: Feature-Sliced Design (FSD) with Atomic Design principles
- **Database**: Entity Framework Core with Code-First migrations

## Backend Technology Stack

### Core Framework
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core 9** - ORM for database operations
- **SQL Server** - Primary database (PostgreSQL alternative)
- **Redis** - Caching layer
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **MediatR** - CQRS implementation

### Authentication & Security
- **JWT Bearer Authentication** with refresh tokens
- **ASP.NET Core Identity** for user management
- **Role-based authorization** with custom policies

### Real-time & Background Processing
- **SignalR** - Real-time communication (WebSocket)
- **Hangfire** - Background job processing

### Logging & Monitoring
- **Serilog** - Structured logging
- **Application Insights** or **Seq** for log aggregation

## Frontend Technology Stack

### Core Framework
- **React 18+** with **TypeScript 5.3+**
- **Vite 5.0** - Build tool and dev server
- **React Router v6** - Client-side routing

### State Management
- **Redux Toolkit (RTK)** - Primary state management
- **RTK Query** - Server state and caching
- **Redux Persist** - State persistence

### UI Framework
- **Material-UI (MUI) v5** - Component library
- **Emotion** - CSS-in-JS styling
- **React Hook Form + Yup** - Form handling and validation

### Data Visualization
- **Recharts** or **Chart.js** - Charts and analytics
- **D3.js** - Advanced visualizations

### Real-time Communication
- **SignalR Client** - WebSocket connection to backend
- **Axios** - HTTP client with interceptors

## Project Structure

### Backend Solution Structure
```
WorkoutProject.sln
├── WorkoutProject.Domain/          # Domain entities, value objects, interfaces
├── WorkoutProject.Application/     # Use cases, DTOs, handlers, services
├── WorkoutProject.Infrastructure/  # Data access, external services
├── WorkoutProject.Presentation/    # API controllers, SignalR hubs
├── WorkoutProject.Shared/          # Cross-cutting concerns
└── workoutproject.client/          # React frontend application
```

### Frontend Structure
```
src/
├── app/                    # App initialization, store, providers
├── entities/              # Business entities (user, workout, exercise)
├── features/              # Feature-specific logic (auth, training, analytics)
├── pages/                 # Route-level components
├── shared/                # Reusable components, hooks, utilities
└── widgets/               # Complex UI blocks (header, sidebar, modals)
```

## Common Build Commands

### Backend Commands
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API (from Presentation project)
dotnet run --project WorkoutProject.Presentation

# Run tests
dotnet test

# Create migration
dotnet ef migrations add MigrationName --project WorkoutProject.Infrastructure

# Update database
dotnet ef database update --project WorkoutProject.Infrastructure

# Generate API client
dotnet swagger tofile --output swagger.json WorkoutProject.Presentation.dll v1
```

### Frontend Commands
```bash
# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Run tests
npm run test

# Run E2E tests
npm run test:e2e

# Lint code
npm run lint

# Format code
npm run format

# Type check
npm run type-check
```

### Docker Commands
```bash
# Build and run with Docker Compose
docker-compose up --build

# Run only database
docker-compose up db redis

# Clean up containers
docker-compose down -v
```

## Development Environment Setup

### Prerequisites
- **.NET 9 SDK**
- **Node.js 18+** and **npm 9+**
- **SQL Server** (or Docker for local development)
- **Redis** (optional, for caching)

### Environment Variables
```bash
# Backend (.NET)
ConnectionStrings__DefaultConnection="Server=localhost;Database=WorkoutProjectDb;Trusted_Connection=true;"
JwtSettings__Secret="your-jwt-secret-key"
RedisSettings__ConnectionString="localhost:6379"

# Frontend (React)
VITE_API_BASE_URL=http://localhost:7207/api
VITE_WEBSOCKET_URL=ws://localhost:7207/hub
```

## Code Quality & Standards

### Backend Standards
- **Clean Architecture** layers with proper dependency injection
- **CQRS pattern** for separating read/write operations
- **Repository pattern** with Unit of Work
- **Domain events** for cross-cutting concerns
- **Async/await** for all I/O operations

### Frontend Standards
- **TypeScript strict mode** enabled
- **Functional components** with hooks only
- **Custom hooks** for business logic reuse
- **Memoization** for performance optimization
- **Error boundaries** for error handling

### Testing Strategy
- **Unit tests**: 80%+ code coverage
- **Integration tests**: API endpoints and database operations
- **E2E tests**: Critical user workflows
- **Component tests**: React components with Testing Library