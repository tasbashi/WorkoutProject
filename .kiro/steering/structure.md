# Project Structure & Organization

## Solution Architecture

The project follows a **Clean Architecture** pattern with clear separation of concerns across multiple layers.

### Backend Project Structure

```
WorkoutProject.sln
├── WorkoutProject.Domain/              # Core business logic (innermost layer)
│   ├── Entities/                      # Domain entities (User, Workout, Exercise, etc.)
│   ├── ValueObjects/                  # Value objects (Measurement, Money, etc.)
│   ├── Enums/                         # Domain enumerations
│   ├── Interfaces/                    # Domain service interfaces
│   ├── Exceptions/                    # Domain-specific exceptions
│   └── Events/                        # Domain events
├── WorkoutProject.Application/         # Application business rules
│   ├── Commands/                      # CQRS command definitions
│   ├── Queries/                       # CQRS query definitions
│   ├── Handlers/                      # Command and query handlers
│   ├── DTOs/                          # Data transfer objects
│   ├── Interfaces/                    # Application service interfaces
│   ├── Services/                      # Application services
│   ├── Mappings/                      # AutoMapper profiles
│   └── Validators/                    # FluentValidation rules
├── WorkoutProject.Infrastructure/      # External concerns
│   ├── Data/                          # EF Core DbContext and configurations
│   ├── Repositories/                  # Repository implementations
│   ├── Services/                      # External service implementations
│   ├── Configurations/                # Entity configurations
│   └── Seeders/                       # Database seed data
├── WorkoutProject.Presentation/        # API layer
│   ├── Controllers/                   # Web API controllers
│   ├── Hubs/                          # SignalR hubs
│   ├── Middlewares/                   # Custom middleware
│   ├── Filters/                       # Action filters
│   └── Extensions/                    # Service registration extensions
├── WorkoutProject.Shared/              # Cross-cutting concerns
│   ├── Constants/                     # Application constants
│   ├── Helpers/                       # Helper classes
│   ├── Models/                        # Shared models
│   └── Extensions/                    # Extension methods
└── workoutproject.client/              # React frontend application
```

### Frontend Project Structure

The frontend follows **Feature-Sliced Design (FSD)** architecture:

```
workoutproject.client/src/
├── app/                               # Application layer
│   ├── providers/                     # React context providers
│   ├── store/                         # Redux store configuration
│   ├── styles/                        # Global styles and theme
│   └── router/                        # Routing configuration
├── entities/                          # Business entities
│   ├── user/                          # User entity and related logic
│   │   ├── api/                       # User API calls
│   │   ├── model/                     # User types and interfaces
│   │   └── ui/                        # User-specific UI components
│   ├── workout/                       # Workout entity
│   ├── exercise/                      # Exercise catalog
│   ├── nutrition/                     # Nutrition tracking
│   └── measurement/                   # Body measurements
├── features/                          # Feature-specific business logic
│   ├── auth/                          # Authentication flows
│   │   ├── api/                       # Auth API calls
│   │   ├── model/                     # Auth state and types
│   │   └── ui/                        # Login, register components
│   ├── training/                      # Training program management
│   ├── analytics/                     # Performance analytics
│   ├── messaging/                     # Communication features
│   └── payments/                      # Subscription handling
├── pages/                             # Route-level components
│   ├── dashboard/                     # Main dashboard views
│   ├── workouts/                      # Workout management pages
│   ├── athletes/                      # Athlete management (trainers)
│   ├── nutrition/                     # Nutrition tracking pages
│   └── settings/                      # Application settings
├── shared/                            # Shared resources
│   ├── api/                           # API client and utilities
│   ├── components/                    # Reusable UI components
│   │   ├── atoms/                     # Basic components (Button, Input)
│   │   ├── molecules/                 # Composite components (SearchBar, Card)
│   │   └── organisms/                 # Complex components (DataTable, Header)
│   ├── hooks/                         # Custom React hooks
│   ├── lib/                           # Third-party library configurations
│   ├── utils/                         # Utility functions
│   └── constants/                     # Application constants
└── widgets/                           # Complex UI blocks
    ├── header/                        # Application header
    ├── sidebar/                       # Navigation sidebar
    ├── modals/                        # Modal components
    └── charts/                        # Chart widgets
```

## Naming Conventions

### Backend (.NET)
- **Namespaces**: PascalCase following folder structure
- **Classes**: PascalCase (e.g., `WorkoutService`, `UserRepository`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `IWorkoutService`)
- **Methods**: PascalCase (e.g., `GetWorkoutByIdAsync`)
- **Properties**: PascalCase (e.g., `FirstName`, `CreatedAt`)
- **Fields**: camelCase with underscore prefix (e.g., `_workoutRepository`)
- **Constants**: PascalCase (e.g., `MaxWorkoutDuration`)
- **Enums**: PascalCase for enum and values (e.g., `WorkoutStatus.InProgress`)

### Frontend (TypeScript/React)
- **Components**: PascalCase (e.g., `WorkoutCard`, `UserProfile`)
- **Files**: PascalCase for components, camelCase for utilities
- **Hooks**: camelCase with 'use' prefix (e.g., `useWorkout`, `useAuth`)
- **Variables/Functions**: camelCase (e.g., `workoutData`, `handleSubmit`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `API_BASE_URL`)
- **Types/Interfaces**: PascalCase (e.g., `User`, `WorkoutData`)
- **Folders**: kebab-case (e.g., `workout-card`, `user-profile`)

### Database
- **Tables**: PascalCase (e.g., `Users`, `WorkoutSessions`)
- **Columns**: PascalCase (e.g., `FirstName`, `CreatedAt`)
- **Primary Keys**: `Id` (GUID)
- **Foreign Keys**: `{TableName}Id` (e.g., `UserId`, `WorkoutId`)
- **Indexes**: `IX_{TableName}_{ColumnName}`
- **Constraints**: `CK_{TableName}_{ColumnName}` for check, `FK_{TableName}_{ReferencedTable}` for foreign key

## File Organization Patterns

### Backend File Structure
```
Feature/
├── Commands/
│   ├── CreateFeatureCommand.cs
│   ├── UpdateFeatureCommand.cs
│   └── DeleteFeatureCommand.cs
├── Queries/
│   ├── GetFeatureQuery.cs
│   └── GetFeaturesQuery.cs
├── Handlers/
│   ├── CreateFeatureHandler.cs
│   └── GetFeatureHandler.cs
├── DTOs/
│   ├── FeatureDto.cs
│   └── CreateFeatureDto.cs
└── Validators/
    └── CreateFeatureValidator.cs
```

### Frontend Feature Structure
```
feature-name/
├── api/
│   └── featureApi.ts
├── model/
│   ├── types.ts
│   ├── store.ts
│   └── selectors.ts
├── ui/
│   ├── FeatureList/
│   │   ├── FeatureList.tsx
│   │   ├── FeatureList.test.tsx
│   │   └── index.ts
│   └── FeatureCard/
└── hooks/
    └── useFeature.ts
```

## Import/Export Conventions

### Backend
- Use explicit imports for better IntelliSense
- Group using statements: System, Microsoft, Third-party, Project
- Use global using statements in GlobalUsings.cs for common imports

### Frontend
- Use barrel exports (index.ts files) for clean imports
- Group imports: React, Third-party libraries, Internal modules
- Use absolute imports for shared resources
- Use relative imports within the same feature

```typescript
// Import order example
import React from 'react';
import { Button, TextField } from '@mui/material';
import { useAppSelector } from '@/shared/hooks';
import { WorkoutCard } from './WorkoutCard';
```

## Component Organization

### Atomic Design Structure
- **Atoms**: Basic building blocks (Button, Input, Icon)
- **Molecules**: Simple combinations (SearchBar, FormField)
- **Organisms**: Complex combinations (Header, DataTable)
- **Templates**: Page layouts (DashboardLayout, AuthLayout)
- **Pages**: Specific instances (Dashboard, WorkoutList)

### Component File Structure
```
ComponentName/
├── ComponentName.tsx          # Main component
├── ComponentName.test.tsx     # Unit tests
├── ComponentName.stories.tsx  # Storybook stories (if applicable)
├── ComponentName.styles.ts    # Styled components
├── types.ts                   # Component-specific types
└── index.ts                   # Barrel export
```

## API Endpoint Organization

### RESTful URL Structure
- **Collections**: `/api/v1/workouts`
- **Resources**: `/api/v1/workouts/{id}`
- **Sub-resources**: `/api/v1/workouts/{id}/exercises`
- **Actions**: `/api/v1/workouts/{id}/assign`

### Controller Organization
- One controller per aggregate root
- Group related actions in the same controller
- Use consistent HTTP verbs (GET, POST, PUT, DELETE)
- Follow RESTful conventions for URL patterns

## Testing Organization

### Backend Tests
```
Tests/
├── UnitTests/
│   ├── Domain/
│   ├── Application/
│   └── Infrastructure/
├── IntegrationTests/
│   ├── Controllers/
│   └── Repositories/
└── TestUtilities/
    ├── Builders/
    └── Fixtures/
```

### Frontend Tests
```
src/
├── __tests__/                 # Global test utilities
├── features/
│   └── auth/
│       ├── __tests__/         # Feature-specific tests
│       └── components/
│           └── Login.test.tsx # Component tests
└── shared/
    └── components/
        └── Button/
            └── Button.test.tsx
```

This structure ensures maintainability, scalability, and clear separation of concerns across the entire application.