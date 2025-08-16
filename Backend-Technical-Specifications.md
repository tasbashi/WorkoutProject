# Backend Technical Specifications
## Coach-Athlete Tracking System

---

## Table of Contents
1. [System Architecture](#1-system-architecture)
2. [Technology Stack](#2-technology-stack)
3. [Domain Model](#3-domain-model)
4. [API Design](#4-api-design)
5. [Data Layer](#5-data-layer)
6. [Authentication & Security](#6-authentication--security)
7. [Business Logic](#7-business-logic)
8. [Real-time Features](#8-real-time-features)
9. [Performance & Scalability](#9-performance--scalability)
10. [Monitoring & Logging](#10-monitoring--logging)

---

## 1. System Architecture

### 1.1 Architecture Pattern
**Clean Architecture** with **CQRS** and **Domain-Driven Design (DDD)** principles

```
Solution Structure:
├── WorkoutProject.Domain/          # Domain layer
│   ├── Entities/                  # Domain entities
│   ├── ValueObjects/              # Value objects
│   ├── Enums/                     # Domain enums
│   ├── Interfaces/                # Domain interfaces
│   ├── Exceptions/                # Domain exceptions
│   └── Events/                    # Domain events
├── WorkoutProject.Application/     # Application layer
│   ├── Commands/                  # CQRS commands
│   ├── Queries/                   # CQRS queries
│   ├── Handlers/                  # Command/query handlers
│   ├── DTOs/                      # Data transfer objects
│   ├── Interfaces/                # Application interfaces
│   ├── Services/                  # Application services
│   ├── Mappings/                  # AutoMapper profiles
│   └── Validators/                # FluentValidation rules
├── WorkoutProject.Infrastructure/ # Infrastructure layer
│   ├── Data/                      # EF Core DbContext
│   ├── Repositories/              # Repository implementations
│   ├── Services/                  # External service implementations
│   ├── Configurations/            # Entity configurations
│   └── Seeders/                   # Database seeders
├── WorkoutProject.Presentation/   # Presentation layer
│   ├── Controllers/               # API controllers
│   ├── Hubs/                      # SignalR hubs
│   ├── Middlewares/               # Custom middlewares
│   ├── Filters/                   # Action filters
│   └── Extensions/                # Service extensions
└── WorkoutProject.Shared/         # Shared utilities
    ├── Constants/                 # Application constants
    ├── Helpers/                   # Helper classes
    ├── Models/                    # Shared models
    └── Extensions/                # Extension methods
```

### 1.2 Layer Dependencies
```
Presentation → Application → Domain
     ↓              ↓
Infrastructure ←────┘
```

### 1.3 Design Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Dependency Inversion**: Depend on abstractions, not concretions
- **Interface Segregation**: Many specific interfaces over one general
- **Domain-Driven Design**: Business logic encapsulated in domain

## 2. Technology Stack

### 2.1 Core Framework
```xml
<PackageReference Include="Microsoft.AspNetCore.App" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
```

### 2.2 Authentication & Authorization
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
```

### 2.3 CQRS & Mediation
```xml
<PackageReference Include="MediatR" Version="12.2.0" />
<PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="11.1.0" />
```

### 2.4 Validation & Mapping
```xml
<PackageReference Include="FluentValidation" Version="11.8.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

### 2.5 Real-time Communication
```xml
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="8.0.0" />
```

### 2.6 Caching & Background Jobs
```xml
<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="8.0.0" />
<PackageReference Include="Hangfire.Core" Version="1.8.6" />
<PackageReference Include="Hangfire.SqlServer" Version="1.8.6" />
<PackageReference Include="Hangfire.AspNetCore" Version="1.8.6" />
```

### 2.7 Logging & Monitoring
```xml
<PackageReference Include="Serilog.AspNetCore" Version="7.0.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="4.1.0" />
<PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
<PackageReference Include="Serilog.Sinks.Seq" Version="5.2.2" />
```

### 2.8 Testing
```xml
<PackageReference Include="xUnit" Version="2.4.2" />
<PackageReference Include="xUnit.runner.visualstudio" Version="2.4.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Moq" Version="4.20.69" />
```

## 3. Domain Model

### 3.1 Core Entities

#### 3.1.1 User Entity
```csharp
public class User : BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; private set; }
    public virtual Trainer? Trainer { get; private set; }
    public virtual Athlete? Athlete { get; private set; }
    
    private User() { } // EF Core constructor
    
    public User(string username, string email, string firstName, string lastName)
    {
        Username = Guard.Against.NullOrWhiteSpace(username);
        Email = Guard.Against.NullOrWhiteSpace(email);
        FirstName = Guard.Against.NullOrWhiteSpace(firstName);
        LastName = Guard.Against.NullOrWhiteSpace(lastName);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UserRoles = new List<UserRole>();
    }
    
    public void UpdateProfile(string firstName, string lastName, DateTime? dateOfBirth, Gender gender)
    {
        FirstName = Guard.Against.NullOrWhiteSpace(firstName);
        LastName = Guard.Against.NullOrWhiteSpace(lastName);
        DateOfBirth = dateOfBirth;
        Gender = gender;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new UserProfileUpdatedEvent(Id));
    }
}
```

#### 3.1.2 Workout Entity
```csharp
public class Workout : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid TrainerId { get; private set; }
    public Guid? AthleteId { get; private set; }
    public WorkoutType Type { get; private set; }
    public WorkoutStatus Status { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public int EstimatedDurationMinutes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Navigation properties
    public virtual Trainer Trainer { get; private set; }
    public virtual Athlete? Athlete { get; private set; }
    public virtual IReadOnlyCollection<WorkoutExercise> Exercises => _exercises.AsReadOnly();
    public virtual IReadOnlyCollection<WorkoutSession> Sessions => _sessions.AsReadOnly();
    
    private readonly List<WorkoutExercise> _exercises = new();
    private readonly List<WorkoutSession> _sessions = new();
    
    private Workout() { } // EF Core constructor
    
    public Workout(string name, Guid trainerId, WorkoutType type, DateTime scheduledDate)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        TrainerId = Guard.Against.Default(trainerId);
        Type = type;
        ScheduledDate = scheduledDate;
        Status = WorkoutStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new WorkoutCreatedEvent(Id, TrainerId));
    }
    
    public void AddExercise(Guid exerciseId, int sets, int reps, decimal? weight, int? restTimeSeconds)
    {
        Guard.Against.Default(exerciseId);
        Guard.Against.NegativeOrZero(sets);
        Guard.Against.NegativeOrZero(reps);
        
        var workoutExercise = new WorkoutExercise(Id, exerciseId, sets, reps, weight, restTimeSeconds);
        _exercises.Add(workoutExercise);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AssignToAthlete(Guid athleteId)
    {
        AthleteId = Guard.Against.Default(athleteId);
        Status = WorkoutStatus.Assigned;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new WorkoutAssignedEvent(Id, athleteId));
    }
    
    public WorkoutSession StartSession(Guid athleteId)
    {
        if (Status != WorkoutStatus.Assigned && Status != WorkoutStatus.InProgress)
            throw new DomainException("Cannot start session for workout in current status");
            
        var session = new WorkoutSession(Id, athleteId);
        _sessions.Add(session);
        Status = WorkoutStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new WorkoutSessionStartedEvent(Id, session.Id, athleteId));
        return session;
    }
}
```

### 3.2 Value Objects

#### 3.2.1 Measurement Value Object
```csharp
public class Measurement : ValueObject
{
    public decimal Value { get; }
    public MeasurementUnit Unit { get; }
    
    public Measurement(decimal value, MeasurementUnit unit)
    {
        Value = Guard.Against.Negative(value);
        Unit = unit;
    }
    
    public Measurement ConvertTo(MeasurementUnit targetUnit)
    {
        if (Unit == targetUnit) return this;
        
        return Unit switch
        {
            MeasurementUnit.Kilograms when targetUnit == MeasurementUnit.Pounds => 
                new Measurement(Value * 2.20462m, targetUnit),
            MeasurementUnit.Pounds when targetUnit == MeasurementUnit.Kilograms => 
                new Measurement(Value / 2.20462m, targetUnit),
            _ => throw new ArgumentException($"Cannot convert from {Unit} to {targetUnit}")
        };
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Unit;
    }
}
```

### 3.3 Domain Events

#### 3.3.1 Base Domain Event
```csharp
public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public class WorkoutCreatedEvent : DomainEvent
{
    public Guid WorkoutId { get; }
    public Guid TrainerId { get; }
    
    public WorkoutCreatedEvent(Guid workoutId, Guid trainerId)
    {
        WorkoutId = workoutId;
        TrainerId = trainerId;
    }
}
```

## 4. API Design

### 4.1 RESTful API Principles
- **Resources**: Nouns representing business entities
- **HTTP Verbs**: Standard HTTP methods (GET, POST, PUT, DELETE)
- **Status Codes**: Proper HTTP status codes
- **Versioning**: URL-based versioning (/api/v1/)
- **Pagination**: Consistent pagination strategy
- **Filtering**: Query parameter-based filtering

### 4.2 API Response Format
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public ICollection<string>? Errors { get; set; }
    public PaginationMetadata? Pagination { get; set; }
}

public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}
```

### 4.3 Controller Base Class
```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public abstract class BaseController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BaseController> _logger;
    
    protected BaseController(IMediator mediator, ILogger<BaseController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    protected async Task<IActionResult> HandleCommand<T>(IRequest<T> command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<T> { Success = true, Data = result });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<T> 
            { 
                Success = false, 
                Errors = ex.Errors.Select(e => e.ErrorMessage).ToList() 
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<T> { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request");
            return StatusCode(500, new ApiResponse<T> 
            { 
                Success = false, 
                Message = "An internal server error occurred" 
            });
        }
    }
}
```

## 5. Data Layer

### 5.1 DbContext Configuration
```csharp
public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutDbContext).Assembly);
        
        // Configure domain events
        ConfigureDomainEvents(modelBuilder);
        
        // Configure soft delete
        ConfigureSoftDelete(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Handle audit fields
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        
        // Dispatch domain events
        await DispatchDomainEventsAsync();
        
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();
            
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
            
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
        
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
```

### 5.2 Entity Configuration Example
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasAnnotation("MinLength", 3);
            
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(254);
            
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);
            
        builder.Property(u => u.Gender)
            .HasConversion<int>();
            
        // Indexes
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        
        // Relationships
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);
            
        builder.HasOne(u => u.Trainer)
            .WithOne(t => t.User)
            .HasForeignKey<Trainer>(t => t.UserId);
            
        builder.HasOne(u => u.Athlete)
            .WithOne(a => a.User)
            .HasForeignKey<Athlete>(a => a.UserId);
    }
}
```

### 5.3 Repository Pattern
```csharp
public interface IRepository<T> where T : BaseEntity, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}

public class Repository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
{
    protected readonly WorkoutDbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public Repository(WorkoutDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    
    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }
    
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }
    
    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }
    
    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
```

## 6. Authentication & Security

### 6.1 JWT Configuration
```csharp
public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

public interface IJwtService
{
    Task<AuthResult> GenerateTokenAsync(User user);
    Task<AuthResult> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
    ClaimsPrincipal? ValidateToken(string token);
}

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public async Task<AuthResult> GenerateTokenAsync(User user)
    {
        var claims = await GenerateClaimsAsync(user);
        var accessToken = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();
        
        await SaveRefreshTokenAsync(user.Id, refreshToken);
        
        return new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _jwtSettings.ExpirationMinutes * 60
        };
    }
    
    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### 6.2 Authorization Policies
```csharp
public static class Policies
{
    public const string RequireTrainerRole = "RequireTrainerRole";
    public const string RequireAthleteRole = "RequireAthleteRole";
    public const string RequireAdminRole = "RequireAdminRole";
    public const string RequireOwnership = "RequireOwnership";
}

public class OwnershipRequirement : IAuthorizationRequirement
{
    public OwnershipRequirement(string resourceIdParameterName = "id")
    {
        ResourceIdParameterName = resourceIdParameterName;
    }
    
    public string ResourceIdParameterName { get; }
}

public class OwnershipHandler : AuthorizationHandler<OwnershipRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnershipRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;
        
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return;
        
        var resourceId = httpContext.Request.RouteValues[requirement.ResourceIdParameterName]?.ToString();
        if (string.IsNullOrEmpty(resourceId)) return;
        
        // Check ownership based on resource type
        var isOwner = await CheckOwnershipAsync(userId, resourceId, httpContext);
        
        if (isOwner)
        {
            context.Succeed(requirement);
        }
    }
}
```

## 7. Business Logic

### 7.1 CQRS Implementation

#### 7.1.1 Command Example
```csharp
public class CreateWorkoutCommand : IRequest<WorkoutDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkoutType Type { get; set; }
    public DateTime ScheduledDate { get; set; }
    public Guid? AthleteId { get; set; }
    public List<CreateWorkoutExerciseDto> Exercises { get; set; } = new();
}

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public async Task<WorkoutDto> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        // Get current trainer
        var trainerId = _currentUserService.GetCurrentUserId();
        var trainer = await _trainerRepository.GetByUserIdAsync(trainerId, cancellationToken);
        
        if (trainer == null)
            throw new UnauthorizedException("Only trainers can create workouts");
        
        // Create workout
        var workout = new Workout(request.Name, trainer.Id, request.Type, request.ScheduledDate);
        
        if (!string.IsNullOrEmpty(request.Description))
            workout.UpdateDescription(request.Description);
        
        // Add exercises
        foreach (var exerciseDto in request.Exercises)
        {
            workout.AddExercise(
                exerciseDto.ExerciseId,
                exerciseDto.Sets,
                exerciseDto.Reps,
                exerciseDto.Weight,
                exerciseDto.RestTimeSeconds
            );
        }
        
        // Assign to athlete if specified
        if (request.AthleteId.HasValue)
        {
            workout.AssignToAthlete(request.AthleteId.Value);
        }
        
        await _workoutRepository.AddAsync(workout, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<WorkoutDto>(workout);
    }
}
```

#### 7.1.2 Query Example
```csharp
public class GetWorkoutsQuery : IRequest<PagedResult<WorkoutDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public WorkoutType? Type { get; set; }
    public WorkoutStatus? Status { get; set; }
    public Guid? AthleteId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SearchTerm { get; set; }
}

public class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, PagedResult<WorkoutDto>>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    
    public async Task<PagedResult<WorkoutDto>> Handle(GetWorkoutsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetCurrentUserId();
        var userRole = _currentUserService.GetCurrentUserRole();
        
        // Build query based on user role and filters
        var query = _workoutRepository.GetQueryable();
        
        // Apply role-based filtering
        if (userRole == UserRole.Athlete)
        {
            query = query.Where(w => w.AthleteId == currentUserId);
        }
        else if (userRole == UserRole.Trainer)
        {
            query = query.Where(w => w.TrainerId == currentUserId);
        }
        
        // Apply filters
        if (request.Type.HasValue)
            query = query.Where(w => w.Type == request.Type.Value);
            
        if (request.Status.HasValue)
            query = query.Where(w => w.Status == request.Status.Value);
            
        if (request.AthleteId.HasValue)
            query = query.Where(w => w.AthleteId == request.AthleteId.Value);
            
        if (request.FromDate.HasValue)
            query = query.Where(w => w.ScheduledDate >= request.FromDate.Value);
            
        if (request.ToDate.HasValue)
            query = query.Where(w => w.ScheduledDate <= request.ToDate.Value);
            
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(w => w.Name.Contains(request.SearchTerm) || 
                                   w.Description.Contains(request.SearchTerm));
        }
        
        // Apply pagination
        var totalCount = await query.CountAsync(cancellationToken);
        var workouts = await query
            .OrderByDescending(w => w.ScheduledDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        var workoutDtos = _mapper.Map<List<WorkoutDto>>(workouts);
        
        return new PagedResult<WorkoutDto>
        {
            Items = workoutDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
```

### 7.2 Domain Services
```csharp
public interface IWorkoutService
{
    Task<bool> CanUserAccessWorkoutAsync(Guid userId, Guid workoutId);
    Task<WorkoutProgress> CalculateProgressAsync(Guid athleteId, Guid workoutId);
    Task<IReadOnlyList<Exercise>> GetRecommendedExercisesAsync(Guid athleteId, WorkoutType type);
}

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IWorkoutSessionRepository _sessionRepository;
    private readonly IAthleteRepository _athleteRepository;
    
    public async Task<WorkoutProgress> CalculateProgressAsync(Guid athleteId, Guid workoutId)
    {
        var sessions = await _sessionRepository.GetByAthleteAndWorkoutAsync(athleteId, workoutId);
        
        if (!sessions.Any())
            return new WorkoutProgress { CompletionPercentage = 0 };
        
        var latestSession = sessions.OrderByDescending(s => s.StartedAt).First();
        var totalExercises = latestSession.Workout.Exercises.Count;
        var completedExercises = latestSession.ExerciseLogs.Count(log => log.IsCompleted);
        
        var completionPercentage = totalExercises > 0 
            ? (decimal)completedExercises / totalExercises * 100 
            : 0;
        
        return new WorkoutProgress
        {
            CompletionPercentage = completionPercentage,
            TotalExercises = totalExercises,
            CompletedExercises = completedExercises,
            LastSessionDate = latestSession.StartedAt,
            AverageSessionDuration = sessions.Average(s => s.DurationMinutes ?? 0)
        };
    }
}
```

## 8. Real-time Features

### 8.1 SignalR Hub
```csharp
[Authorize]
public class WorkoutHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    
    public async Task JoinWorkoutSession(string workoutSessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"workout-{workoutSessionId}");
    }
    
    public async Task LeaveWorkoutSession(string workoutSessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"workout-{workoutSessionId}");
    }
    
    public async Task UpdateExerciseProgress(string sessionId, string exerciseId, int completedSets)
    {
        var command = new UpdateExerciseProgressCommand
        {
            SessionId = Guid.Parse(sessionId),
            ExerciseId = Guid.Parse(exerciseId),
            CompletedSets = completedSets
        };
        
        await _mediator.Send(command);
        
        // Notify all clients in the workout session
        await Clients.Group($"workout-{sessionId}")
            .SendAsync("ExerciseProgressUpdated", exerciseId, completedSets);
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = _currentUserService.GetCurrentUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = _currentUserService.GetCurrentUserId();
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
        await base.OnDisconnectedAsync(exception);
    }
}
```

### 8.2 Real-time Notifications
```csharp
public interface INotificationService
{
    Task SendToUserAsync(Guid userId, string message, NotificationType type);
    Task SendToGroupAsync(string groupName, string message, NotificationType type);
    Task SendWorkoutReminderAsync(Guid athleteId, Guid workoutId);
    Task SendProgressUpdateAsync(Guid trainerId, Guid athleteId, string progressSummary);
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationRepository _notificationRepository;
    
    public async Task SendToUserAsync(Guid userId, string message, NotificationType type)
    {
        var notification = new Notification(userId, message, type);
        await _notificationRepository.AddAsync(notification);
        
        await _hubContext.Clients.Group($"user-{userId}")
            .SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Message = message,
                Type = type.ToString(),
                Timestamp = notification.CreatedAt
            });
    }
}
```

## 9. Performance & Scalability

### 9.1 Caching Strategy
```csharp
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
}

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CacheService> _logger;
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cachedValue == null) return null;
            
            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cached value for key: {Key}", key);
            return null;
        }
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var serializedValue = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions();
            
            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);
            else
                options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            
            await _distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching value for key: {Key}", key);
        }
    }
}
```

### 9.2 Background Jobs
```csharp
public interface IBackgroundJobService
{
    void ScheduleWorkoutReminder(Guid athleteId, Guid workoutId, DateTime scheduledDate);
    void ProcessAnalytics(Guid athleteId);
    void GenerateMonthlyReport(Guid trainerId, int year, int month);
}

public class BackgroundJobService : IBackgroundJobService
{
    public void ScheduleWorkoutReminder(Guid athleteId, Guid workoutId, DateTime scheduledDate)
    {
        var reminderTime = scheduledDate.AddHours(-1); // 1 hour before
        
        BackgroundJob.Schedule<INotificationService>(
            service => service.SendWorkoutReminderAsync(athleteId, workoutId),
            reminderTime
        );
    }
    
    public void ProcessAnalytics(Guid athleteId)
    {
        BackgroundJob.Enqueue<IAnalyticsService>(
            service => service.ProcessAthleteAnalyticsAsync(athleteId)
        );
    }
}
```

## 10. Monitoring & Logging

### 10.1 Structured Logging
```csharp
public static class LoggerExtensions
{
    public static void LogWorkoutCreated(this ILogger logger, Guid workoutId, Guid trainerId)
    {
        logger.LogInformation("Workout {WorkoutId} created by trainer {TrainerId}", workoutId, trainerId);
    }
    
    public static void LogWorkoutSessionStarted(this ILogger logger, Guid sessionId, Guid athleteId)
    {
        logger.LogInformation("Workout session {SessionId} started by athlete {AthleteId}", sessionId, athleteId);
    }
    
    public static void LogPerformanceMetric(this ILogger logger, string operation, long elapsedMs)
    {
        if (elapsedMs > 1000)
        {
            logger.LogWarning("Slow operation detected: {Operation} took {ElapsedMs}ms", operation, elapsedMs);
        }
        else
        {
            logger.LogDebug("Operation {Operation} completed in {ElapsedMs}ms", operation, elapsedMs);
        }
    }
}
```

### 10.2 Health Checks
```csharp
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly WorkoutDbContext _context;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database connection is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection is unhealthy", ex);
        }
    }
}

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDistributedCache _cache;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.SetStringAsync("health-check", "test", cancellationToken);
            var value = await _cache.GetStringAsync("health-check", cancellationToken);
            
            return value == "test" 
                ? HealthCheckResult.Healthy("Redis cache is healthy")
                : HealthCheckResult.Unhealthy("Redis cache returned unexpected value");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis cache is unhealthy", ex);
        }
    }
}
```

---

## Appendix A: Configuration Examples

### Program.cs Configuration
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(typeof(CreateWorkoutCommand));
builder.Services.AddAutoMapper(typeof(WorkoutProfile));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(CreateWorkoutCommandValidator).Assembly);

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireTrainerRole, 
        policy => policy.RequireRole(UserRole.Trainer.ToString()));
    options.AddPolicy(Policies.RequireAthleteRole, 
        policy => policy.RequireRole(UserRole.Athlete.ToString()));
});

// Add SignalR
builder.Services.AddSignalR();

// Add caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<RedisHealthCheck>("redis");

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<WorkoutHub>("/hubs/workout");
app.MapHealthChecks("/health");

app.Run();
```
