# Database Schema Specifications
## Coach-Athlete Tracking System

---

## Table of Contents
1. [Database Overview](#1-database-overview)
2. [Core Entity Tables](#2-core-entity-tables)
3. [Authentication & Authorization](#3-authentication--authorization)
4. [Workout Management](#4-workout-management)
5. [Exercise & Training](#5-exercise--training)
6. [Performance Tracking](#6-performance-tracking)
7. [Nutrition Management](#7-nutrition-management)
8. [Communication System](#8-communication-system)
9. [Analytics & Reporting](#9-analytics--reporting)
10. [System Configuration](#10-system-configuration)

---

## 1. Database Overview

### 1.1 Database Technology
- **Primary Database**: SQL Server 2022
- **Caching Layer**: Redis 7.0+
- **Search Engine**: Elasticsearch 8.0+ (Optional)
- **File Storage**: Azure Blob Storage

### 1.2 Design Principles
- **Normalization**: 3NF with selective denormalization for performance
- **Data Integrity**: Foreign key constraints, check constraints, and triggers
- **Audit Trail**: Created/updated timestamps and user tracking
- **Soft Delete**: Logical deletion for data retention
- **Indexing Strategy**: Optimized for read-heavy workloads
- **Partitioning**: Time-based partitioning for large tables

### 1.3 Naming Conventions
- **Tables**: PascalCase (e.g., `Users`, `WorkoutSessions`)
- **Columns**: PascalCase (e.g., `FirstName`, `CreatedAt`)
- **Primary Keys**: `Id` (GUID)
- **Foreign Keys**: `{TableName}Id` (e.g., `UserId`, `WorkoutId`)
- **Indexes**: `IX_{TableName}_{ColumnName(s)}`
- **Constraints**: `CK_{TableName}_{ColumnName}`, `FK_{TableName}_{ReferencedTable}`

---

## 2. Core Entity Tables

### 2.1 Users Table
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(254) NOT NULL UNIQUE,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    PasswordHash NVARCHAR(255) NOT NULL,
    SecurityStamp NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NULL,
    Gender TINYINT NULL, -- 0: Male, 1: Female, 2: Other
    PhoneNumber NVARCHAR(20) NULL,
    ProfilePictureUrl NVARCHAR(500) NULL,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnabled BIT NOT NULL DEFAULT 1,
    LockoutEnd DATETIMEOFFSET NULL,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    LastLoginAt DATETIMEOFFSET NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NULL,
    UpdatedBy UNIQUEIDENTIFIER NULL,
    
    CONSTRAINT CK_Users_Gender CHECK (Gender IN (0, 1, 2)),
    CONSTRAINT CK_Users_Email CHECK (Email LIKE '%_@_%'),
    CONSTRAINT CK_Users_Username_Length CHECK (LEN(Username) >= 3),
    CONSTRAINT FK_Users_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Users_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(Id)
);

-- Indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_IsActive_IsDeleted ON Users(IsActive, IsDeleted);
CREATE INDEX IX_Users_CreatedAt ON Users(CreatedAt);
```

### 2.2 Roles Table
```sql
CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    NormalizedName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(500) NULL,
    Permissions NVARCHAR(MAX) NULL, -- JSON array of permissions
    IsSystemRole BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT CK_Roles_Name_Length CHECK (LEN(Name) >= 2)
);

-- Default roles data
INSERT INTO Roles (Name, NormalizedName, Description, IsSystemRole) VALUES
('Admin', 'ADMIN', 'System administrator with full access', 1),
('Trainer', 'TRAINER', 'Fitness trainer managing athletes', 1),
('Athlete', 'ATHLETE', 'Individual athlete user', 1),
('Nutritionist', 'NUTRITIONIST', 'Nutrition specialist', 1);
```

### 2.3 UserRoles Table
```sql
CREATE TABLE UserRoles (
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    AssignedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    AssignedBy UNIQUEIDENTIFIER NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_AssignedBy FOREIGN KEY (AssignedBy) REFERENCES Users(Id)
);

CREATE INDEX IX_UserRoles_UserId ON UserRoles(UserId);
CREATE INDEX IX_UserRoles_RoleId ON UserRoles(RoleId);
```

### 2.4 Trainers Table
```sql
CREATE TABLE Trainers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    Specializations NVARCHAR(500) NULL, -- JSON array
    Certifications NVARCHAR(MAX) NULL, -- JSON array of certification objects
    Experience NVARCHAR(1000) NULL,
    Bio NVARCHAR(2000) NULL,
    HourlyRate DECIMAL(10,2) NULL,
    Currency NVARCHAR(3) NULL DEFAULT 'USD',
    IsVerified BIT NOT NULL DEFAULT 0,
    VerificationDate DATETIMEOFFSET NULL,
    Rating DECIMAL(3,2) NULL, -- Average rating 0.00-5.00
    TotalReviews INT NOT NULL DEFAULT 0,
    MaxAthletes INT NULL DEFAULT 50,
    IsAcceptingNewAthletes BIT NOT NULL DEFAULT 1,
    TimeZone NVARCHAR(50) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Trainers_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Trainers_Rating CHECK (Rating >= 0 AND Rating <= 5),
    CONSTRAINT CK_Trainers_HourlyRate CHECK (HourlyRate >= 0)
);

CREATE INDEX IX_Trainers_UserId ON Trainers(UserId);
CREATE INDEX IX_Trainers_IsVerified ON Trainers(IsVerified);
CREATE INDEX IX_Trainers_Rating ON Trainers(Rating);
```

### 2.5 Athletes Table
```sql
CREATE TABLE Athletes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    TrainerId UNIQUEIDENTIFIER NULL,
    Height DECIMAL(5,2) NULL, -- in cm
    Weight DECIMAL(5,2) NULL, -- in kg
    ActivityLevel TINYINT NULL, -- 0: Sedentary, 1: Light, 2: Moderate, 3: Active, 4: Very Active
    Goals NVARCHAR(1000) NULL, -- JSON array
    MedicalHistory NVARCHAR(2000) NULL,
    Allergies NVARCHAR(1000) NULL,
    EmergencyContactName NVARCHAR(100) NULL,
    EmergencyContactPhone NVARCHAR(20) NULL,
    EmergencyContactRelationship NVARCHAR(50) NULL,
    JoinedTrainerAt DATETIMEOFFSET NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Athletes_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Athletes_Trainers FOREIGN KEY (TrainerId) REFERENCES Trainers(Id),
    CONSTRAINT CK_Athletes_ActivityLevel CHECK (ActivityLevel BETWEEN 0 AND 4),
    CONSTRAINT CK_Athletes_Height CHECK (Height > 0 AND Height <= 300),
    CONSTRAINT CK_Athletes_Weight CHECK (Weight > 0 AND Weight <= 1000)
);

CREATE INDEX IX_Athletes_UserId ON Athletes(UserId);
CREATE INDEX IX_Athletes_TrainerId ON Athletes(TrainerId);
CREATE INDEX IX_Athletes_IsActive ON Athletes(IsActive);
```

---

## 3. Authentication & Authorization

### 3.1 RefreshTokens Table
```sql
CREATE TABLE RefreshTokens (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Token NVARCHAR(255) NOT NULL UNIQUE,
    JwtId NVARCHAR(255) NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    IsRevoked BIT NOT NULL DEFAULT 0,
    ExpiryDate DATETIMEOFFSET NOT NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    RevokedAt DATETIMEOFFSET NULL,
    RevokedByIp NVARCHAR(45) NULL,
    ReplacedByToken NVARCHAR(255) NULL,
    
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IX_RefreshTokens_Token ON RefreshTokens(Token);
CREATE INDEX IX_RefreshTokens_UserId ON RefreshTokens(UserId);
CREATE INDEX IX_RefreshTokens_ExpiryDate ON RefreshTokens(ExpiryDate);
```

### 3.2 UserClaims Table
```sql
CREATE TABLE UserClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    ClaimType NVARCHAR(255) NOT NULL,
    ClaimValue NVARCHAR(255) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_UserClaims_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IX_UserClaims_UserId ON UserClaims(UserId);
CREATE INDEX IX_UserClaims_ClaimType ON UserClaims(ClaimType);
```

### 3.3 UserLogins Table
```sql
CREATE TABLE UserLogins (
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    ProviderDisplayName NVARCHAR(255) NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_UserLogins_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IX_UserLogins_UserId ON UserLogins(UserId);
```

---

## 4. Workout Management

### 4.1 Exercises Table
```sql
CREATE TABLE Exercises (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Category NVARCHAR(50) NOT NULL, -- chest, back, legs, shoulders, arms, core, cardio
    MuscleGroups NVARCHAR(500) NOT NULL, -- JSON array
    Equipment NVARCHAR(100) NULL, -- barbell, dumbbell, machine, bodyweight, etc.
    Difficulty TINYINT NOT NULL, -- 1: Beginner, 2: Intermediate, 3: Advanced
    Instructions NVARCHAR(MAX) NOT NULL,
    Tips NVARCHAR(MAX) NULL, -- JSON array
    ImageUrl NVARCHAR(500) NULL,
    VideoUrl NVARCHAR(500) NULL,
    Variations NVARCHAR(MAX) NULL, -- JSON array of variation objects
    IsPublic BIT NOT NULL DEFAULT 1,
    CreatedBy UNIQUEIDENTIFIER NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Exercises_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_Exercises_Difficulty CHECK (Difficulty BETWEEN 1 AND 3)
);

CREATE INDEX IX_Exercises_Category ON Exercises(Category);
CREATE INDEX IX_Exercises_Equipment ON Exercises(Equipment);
CREATE INDEX IX_Exercises_Difficulty ON Exercises(Difficulty);
CREATE INDEX IX_Exercises_IsPublic_IsActive ON Exercises(IsPublic, IsActive);
CREATE FULLTEXT INDEX FTI_Exercises_Name_Instructions ON Exercises(Name, Instructions);
```

### 4.2 Workouts Table
```sql
CREATE TABLE Workouts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    TrainerId UNIQUEIDENTIFIER NOT NULL,
    AthleteId UNIQUEIDENTIFIER NULL,
    WorkoutType TINYINT NOT NULL, -- 0: Strength, 1: Cardio, 2: Flexibility, 3: Mixed
    Status TINYINT NOT NULL DEFAULT 0, -- 0: Draft, 1: Assigned, 2: In Progress, 3: Completed, 4: Cancelled
    ScheduledDate DATETIMEOFFSET NOT NULL,
    EstimatedDurationMinutes INT NULL,
    ActualDurationMinutes INT NULL,
    Notes NVARCHAR(1000) NULL,
    IsTemplate BIT NOT NULL DEFAULT 0,
    TemplateId UNIQUEIDENTIFIER NULL, -- Reference to template if created from one
    IsPublic BIT NOT NULL DEFAULT 0,
    CompletedAt DATETIMEOFFSET NULL,
    Rating TINYINT NULL, -- 1-5 athlete rating
    AthleteNotes NVARCHAR(1000) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Workouts_Trainers FOREIGN KEY (TrainerId) REFERENCES Trainers(Id),
    CONSTRAINT FK_Workouts_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id),
    CONSTRAINT FK_Workouts_Templates FOREIGN KEY (TemplateId) REFERENCES Workouts(Id),
    CONSTRAINT CK_Workouts_WorkoutType CHECK (WorkoutType BETWEEN 0 AND 3),
    CONSTRAINT CK_Workouts_Status CHECK (Status BETWEEN 0 AND 4),
    CONSTRAINT CK_Workouts_Rating CHECK (Rating BETWEEN 1 AND 5),
    CONSTRAINT CK_Workouts_EstimatedDuration CHECK (EstimatedDurationMinutes > 0),
    CONSTRAINT CK_Workouts_ActualDuration CHECK (ActualDurationMinutes > 0)
);

CREATE INDEX IX_Workouts_TrainerId ON Workouts(TrainerId);
CREATE INDEX IX_Workouts_AthleteId ON Workouts(AthleteId);
CREATE INDEX IX_Workouts_ScheduledDate ON Workouts(ScheduledDate);
CREATE INDEX IX_Workouts_Status ON Workouts(Status);
CREATE INDEX IX_Workouts_IsTemplate ON Workouts(IsTemplate);
CREATE INDEX IX_Workouts_WorkoutType ON Workouts(WorkoutType);
```

### 4.3 WorkoutExercises Table
```sql
CREATE TABLE WorkoutExercises (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    WorkoutId UNIQUEIDENTIFIER NOT NULL,
    ExerciseId UNIQUEIDENTIFIER NOT NULL,
    OrderIndex INT NOT NULL,
    Sets INT NOT NULL,
    Reps INT NULL,
    Weight DECIMAL(6,2) NULL, -- in kg
    Duration INT NULL, -- in seconds for time-based exercises
    Distance DECIMAL(8,2) NULL, -- in meters for distance-based exercises
    RestTimeSeconds INT NULL,
    RPE TINYINT NULL, -- Rate of Perceived Exertion 1-10
    Notes NVARCHAR(500) NULL,
    IsSuperset BIT NOT NULL DEFAULT 0,
    SupersetGroup TINYINT NULL,
    IsDropset BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_WorkoutExercises_Workouts FOREIGN KEY (WorkoutId) REFERENCES Workouts(Id) ON DELETE CASCADE,
    CONSTRAINT FK_WorkoutExercises_Exercises FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id),
    CONSTRAINT CK_WorkoutExercises_Sets CHECK (Sets > 0),
    CONSTRAINT CK_WorkoutExercises_Reps CHECK (Reps IS NULL OR Reps > 0),
    CONSTRAINT CK_WorkoutExercises_Weight CHECK (Weight IS NULL OR Weight >= 0),
    CONSTRAINT CK_WorkoutExercises_Duration CHECK (Duration IS NULL OR Duration > 0),
    CONSTRAINT CK_WorkoutExercises_RPE CHECK (RPE IS NULL OR (RPE BETWEEN 1 AND 10))
);

CREATE INDEX IX_WorkoutExercises_WorkoutId ON WorkoutExercises(WorkoutId);
CREATE INDEX IX_WorkoutExercises_ExerciseId ON WorkoutExercises(ExerciseId);
CREATE INDEX IX_WorkoutExercises_OrderIndex ON WorkoutExercises(WorkoutId, OrderIndex);
```

### 4.4 WorkoutSessions Table
```sql
CREATE TABLE WorkoutSessions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    WorkoutId UNIQUEIDENTIFIER NOT NULL,
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    StartedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    CompletedAt DATETIMEOFFSET NULL,
    DurationMinutes INT NULL,
    CaloriesBurned INT NULL,
    AverageHeartRate INT NULL,
    MaxHeartRate INT NULL,
    RPE TINYINT NULL, -- Overall session RPE
    Notes NVARCHAR(1000) NULL,
    Status TINYINT NOT NULL DEFAULT 0, -- 0: In Progress, 1: Completed, 2: Abandoned
    BodyWeight DECIMAL(5,2) NULL, -- Weight at time of workout
    
    CONSTRAINT FK_WorkoutSessions_Workouts FOREIGN KEY (WorkoutId) REFERENCES Workouts(Id),
    CONSTRAINT FK_WorkoutSessions_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id),
    CONSTRAINT CK_WorkoutSessions_Status CHECK (Status BETWEEN 0 AND 2),
    CONSTRAINT CK_WorkoutSessions_RPE CHECK (RPE IS NULL OR (RPE BETWEEN 1 AND 10)),
    CONSTRAINT CK_WorkoutSessions_HeartRate CHECK (
        (AverageHeartRate IS NULL OR AverageHeartRate > 0) AND
        (MaxHeartRate IS NULL OR MaxHeartRate > 0) AND
        (MaxHeartRate IS NULL OR AverageHeartRate IS NULL OR MaxHeartRate >= AverageHeartRate)
    )
);

CREATE INDEX IX_WorkoutSessions_WorkoutId ON WorkoutSessions(WorkoutId);
CREATE INDEX IX_WorkoutSessions_AthleteId ON WorkoutSessions(AthleteId);
CREATE INDEX IX_WorkoutSessions_StartedAt ON WorkoutSessions(StartedAt);
CREATE INDEX IX_WorkoutSessions_Status ON WorkoutSessions(Status);
```

### 4.5 ExerciseLogs Table
```sql
CREATE TABLE ExerciseLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    WorkoutExerciseId UNIQUEIDENTIFIER NOT NULL,
    SetNumber INT NOT NULL,
    Reps INT NULL,
    Weight DECIMAL(6,2) NULL,
    Duration INT NULL, -- in seconds
    Distance DECIMAL(8,2) NULL, -- in meters
    RPE TINYINT NULL,
    RestTimeSeconds INT NULL,
    Notes NVARCHAR(500) NULL,
    IsCompleted BIT NOT NULL DEFAULT 1,
    CompletedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_ExerciseLogs_WorkoutSessions FOREIGN KEY (SessionId) REFERENCES WorkoutSessions(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ExerciseLogs_WorkoutExercises FOREIGN KEY (WorkoutExerciseId) REFERENCES WorkoutExercises(Id),
    CONSTRAINT CK_ExerciseLogs_SetNumber CHECK (SetNumber > 0),
    CONSTRAINT CK_ExerciseLogs_Reps CHECK (Reps IS NULL OR Reps > 0),
    CONSTRAINT CK_ExerciseLogs_Weight CHECK (Weight IS NULL OR Weight >= 0),
    CONSTRAINT CK_ExerciseLogs_RPE CHECK (RPE IS NULL OR (RPE BETWEEN 1 AND 10))
);

CREATE INDEX IX_ExerciseLogs_SessionId ON ExerciseLogs(SessionId);
CREATE INDEX IX_ExerciseLogs_WorkoutExerciseId ON ExerciseLogs(WorkoutExerciseId);
CREATE INDEX IX_ExerciseLogs_CompletedAt ON ExerciseLogs(CompletedAt);
```

---

## 5. Exercise & Training

### 5.1 TrainingPrograms Table
```sql
CREATE TABLE TrainingPrograms (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ProgramType TINYINT NOT NULL, -- 0: Strength, 1: Cardio, 2: Flexibility, 3: Mixed
    Level TINYINT NOT NULL, -- 1: Beginner, 2: Intermediate, 3: Advanced
    DurationWeeks INT NOT NULL,
    WorkoutsPerWeek INT NOT NULL,
    RestDaysPerWeek INT NOT NULL,
    Tags NVARCHAR(500) NULL, -- JSON array
    IsPublic BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    Price DECIMAL(10,2) NULL,
    Currency NVARCHAR(3) NULL DEFAULT 'USD',
    Rating DECIMAL(3,2) NULL,
    TotalReviews INT NOT NULL DEFAULT 0,
    EnrollmentCount INT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_TrainingPrograms_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_TrainingPrograms_ProgramType CHECK (ProgramType BETWEEN 0 AND 3),
    CONSTRAINT CK_TrainingPrograms_Level CHECK (Level BETWEEN 1 AND 3),
    CONSTRAINT CK_TrainingPrograms_DurationWeeks CHECK (DurationWeeks > 0),
    CONSTRAINT CK_TrainingPrograms_WorkoutsPerWeek CHECK (WorkoutsPerWeek > 0 AND WorkoutsPerWeek <= 7),
    CONSTRAINT CK_TrainingPrograms_RestDaysPerWeek CHECK (RestDaysPerWeek >= 0 AND RestDaysPerWeek <= 7),
    CONSTRAINT CK_TrainingPrograms_Rating CHECK (Rating IS NULL OR (Rating >= 0 AND Rating <= 5))
);

CREATE INDEX IX_TrainingPrograms_CreatedBy ON TrainingPrograms(CreatedBy);
CREATE INDEX IX_TrainingPrograms_ProgramType ON TrainingPrograms(ProgramType);
CREATE INDEX IX_TrainingPrograms_Level ON TrainingPrograms(Level);
CREATE INDEX IX_TrainingPrograms_IsPublic_IsActive ON TrainingPrograms(IsPublic, IsActive);
```

### 5.2 ProgramEnrollments Table
```sql
CREATE TABLE ProgramEnrollments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProgramId UNIQUEIDENTIFIER NOT NULL,
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    TrainerId UNIQUEIDENTIFIER NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Status TINYINT NOT NULL DEFAULT 0, -- 0: Active, 1: Completed, 2: Paused, 3: Cancelled
    CurrentWeek INT NOT NULL DEFAULT 1,
    CompletionPercentage DECIMAL(5,2) NOT NULL DEFAULT 0,
    EnrolledAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    CompletedAt DATETIMEOFFSET NULL,
    
    CONSTRAINT FK_ProgramEnrollments_Programs FOREIGN KEY (ProgramId) REFERENCES TrainingPrograms(Id),
    CONSTRAINT FK_ProgramEnrollments_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id),
    CONSTRAINT FK_ProgramEnrollments_Trainers FOREIGN KEY (TrainerId) REFERENCES Trainers(Id),
    CONSTRAINT CK_ProgramEnrollments_Status CHECK (Status BETWEEN 0 AND 3),
    CONSTRAINT CK_ProgramEnrollments_CurrentWeek CHECK (CurrentWeek > 0),
    CONSTRAINT CK_ProgramEnrollments_CompletionPercentage CHECK (CompletionPercentage >= 0 AND CompletionPercentage <= 100),
    CONSTRAINT UQ_ProgramEnrollments_Athlete_Program UNIQUE (ProgramId, AthleteId)
);

CREATE INDEX IX_ProgramEnrollments_ProgramId ON ProgramEnrollments(ProgramId);
CREATE INDEX IX_ProgramEnrollments_AthleteId ON ProgramEnrollments(AthleteId);
CREATE INDEX IX_ProgramEnrollments_Status ON ProgramEnrollments(Status);
```

---

## 6. Performance Tracking

### 6.1 BodyMeasurements Table
```sql
CREATE TABLE BodyMeasurements (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    MeasurementDate DATE NOT NULL,
    Weight DECIMAL(5,2) NULL, -- kg
    BodyFatPercentage DECIMAL(4,2) NULL,
    MuscleMassKg DECIMAL(5,2) NULL,
    Chest DECIMAL(5,2) NULL, -- cm
    Waist DECIMAL(5,2) NULL, -- cm
    Hips DECIMAL(5,2) NULL, -- cm
    Biceps DECIMAL(5,2) NULL, -- cm
    Thighs DECIMAL(5,2) NULL, -- cm
    Neck DECIMAL(5,2) NULL, -- cm
    Shoulders DECIMAL(5,2) NULL, -- cm
    BMI DECIMAL(4,2) NULL,
    VisceralFat DECIMAL(4,2) NULL,
    Notes NVARCHAR(500) NULL,
    MeasuredBy UNIQUEIDENTIFIER NULL, -- User who took measurements
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_BodyMeasurements_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_BodyMeasurements_MeasuredBy FOREIGN KEY (MeasuredBy) REFERENCES Users(Id),
    CONSTRAINT CK_BodyMeasurements_Weight CHECK (Weight IS NULL OR (Weight > 0 AND Weight <= 1000)),
    CONSTRAINT CK_BodyMeasurements_BodyFat CHECK (BodyFatPercentage IS NULL OR (BodyFatPercentage >= 0 AND BodyFatPercentage <= 100)),
    CONSTRAINT UQ_BodyMeasurements_Athlete_Date UNIQUE (AthleteId, MeasurementDate)
);

CREATE INDEX IX_BodyMeasurements_AthleteId ON BodyMeasurements(AthleteId);
CREATE INDEX IX_BodyMeasurements_MeasurementDate ON BodyMeasurements(MeasurementDate);
```

### 6.2 PersonalRecords Table
```sql
CREATE TABLE PersonalRecords (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    ExerciseId UNIQUEIDENTIFIER NOT NULL,
    RecordType TINYINT NOT NULL, -- 0: 1RM, 1: Max Volume, 2: Max Reps, 3: Max Duration, 4: Max Distance
    Value DECIMAL(10,2) NOT NULL,
    Unit NVARCHAR(10) NOT NULL, -- kg, lbs, seconds, meters, etc.
    Reps INT NULL,
    Notes NVARCHAR(500) NULL,
    AchievedAt DATETIMEOFFSET NOT NULL,
    SessionId UNIQUEIDENTIFIER NULL, -- Reference to workout session
    PreviousRecord DECIMAL(10,2) NULL,
    ImprovementPercentage DECIMAL(5,2) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_PersonalRecords_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PersonalRecords_Exercises FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id),
    CONSTRAINT FK_PersonalRecords_Sessions FOREIGN KEY (SessionId) REFERENCES WorkoutSessions(Id),
    CONSTRAINT CK_PersonalRecords_RecordType CHECK (RecordType BETWEEN 0 AND 4),
    CONSTRAINT CK_PersonalRecords_Value CHECK (Value > 0),
    CONSTRAINT UQ_PersonalRecords_Athlete_Exercise_Type UNIQUE (AthleteId, ExerciseId, RecordType)
);

CREATE INDEX IX_PersonalRecords_AthleteId ON PersonalRecords(AthleteId);
CREATE INDEX IX_PersonalRecords_ExerciseId ON PersonalRecords(ExerciseId);
CREATE INDEX IX_PersonalRecords_AchievedAt ON PersonalRecords(AchievedAt);
```

### 6.3 ProgressPhotos Table
```sql
CREATE TABLE ProgressPhotos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    PhotoUrl NVARCHAR(500) NOT NULL,
    PhotoType TINYINT NOT NULL, -- 0: Front, 1: Side, 2: Back, 3: Other
    TakenDate DATE NOT NULL,
    Weight DECIMAL(5,2) NULL,
    BodyFatPercentage DECIMAL(4,2) NULL,
    Notes NVARCHAR(500) NULL,
    IsPublic BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_ProgressPhotos_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id) ON DELETE CASCADE,
    CONSTRAINT CK_ProgressPhotos_PhotoType CHECK (PhotoType BETWEEN 0 AND 3)
);

CREATE INDEX IX_ProgressPhotos_AthleteId ON ProgressPhotos(AthleteId);
CREATE INDEX IX_ProgressPhotos_TakenDate ON ProgressPhotos(TakenDate);
```

---

## 7. Nutrition Management

### 7.1 Foods Table
```sql
CREATE TABLE Foods (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Brand NVARCHAR(100) NULL,
    Barcode NVARCHAR(20) NULL,
    ServingSize DECIMAL(8,2) NOT NULL,
    ServingUnit NVARCHAR(20) NOT NULL, -- grams, ml, cup, piece, etc.
    CaloriesPer100g DECIMAL(7,2) NOT NULL,
    ProteinPer100g DECIMAL(6,2) NOT NULL,
    CarbsPer100g DECIMAL(6,2) NOT NULL,
    FatPer100g DECIMAL(6,2) NOT NULL,
    FiberPer100g DECIMAL(6,2) NULL,
    SugarPer100g DECIMAL(6,2) NULL,
    SodiumPer100g DECIMAL(7,2) NULL, -- mg
    Category NVARCHAR(50) NULL,
    IsVerified BIT NOT NULL DEFAULT 0,
    CreatedBy UNIQUEIDENTIFIER NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Foods_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_Foods_ServingSize CHECK (ServingSize > 0),
    CONSTRAINT CK_Foods_Calories CHECK (CaloriesPer100g >= 0),
    CONSTRAINT CK_Foods_Macros CHECK (
        ProteinPer100g >= 0 AND CarbsPer100g >= 0 AND FatPer100g >= 0
    )
);

CREATE INDEX IX_Foods_Name ON Foods(Name);
CREATE INDEX IX_Foods_Barcode ON Foods(Barcode);
CREATE INDEX IX_Foods_Category ON Foods(Category);
CREATE FULLTEXT INDEX FTI_Foods_Name_Brand ON Foods(Name, Brand);
```

### 7.2 NutritionGoals Table
```sql
CREATE TABLE NutritionGoals (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    CaloriesGoal INT NOT NULL,
    ProteinGoal DECIMAL(6,2) NOT NULL, -- grams
    CarbsGoal DECIMAL(6,2) NOT NULL, -- grams
    FatGoal DECIMAL(6,2) NOT NULL, -- grams
    FiberGoal DECIMAL(6,2) NULL, -- grams
    WaterGoal INT NULL, -- ml
    Goal NVARCHAR(50) NOT NULL, -- weight_loss, weight_gain, maintenance, muscle_gain
    ActivityMultiplier DECIMAL(3,2) NOT NULL DEFAULT 1.2,
    Notes NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_NutritionGoals_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_NutritionGoals_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_NutritionGoals_Calories CHECK (CaloriesGoal > 0),
    CONSTRAINT CK_NutritionGoals_Macros CHECK (
        ProteinGoal > 0 AND CarbsGoal >= 0 AND FatGoal > 0
    )
);

CREATE INDEX IX_NutritionGoals_AthleteId ON NutritionGoals(AthleteId);
CREATE INDEX IX_NutritionGoals_StartDate ON NutritionGoals(StartDate);
```

### 7.3 FoodLogs Table
```sql
CREATE TABLE FoodLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AthleteId UNIQUEIDENTIFIER NOT NULL,
    FoodId UNIQUEIDENTIFIER NOT NULL,
    LogDate DATE NOT NULL,
    MealType NVARCHAR(20) NOT NULL, -- breakfast, lunch, dinner, snack
    Quantity DECIMAL(8,2) NOT NULL,
    Unit NVARCHAR(20) NOT NULL,
    CaloriesConsumed DECIMAL(7,2) NOT NULL,
    ProteinConsumed DECIMAL(6,2) NOT NULL,
    CarbsConsumed DECIMAL(6,2) NOT NULL,
    FatConsumed DECIMAL(6,2) NOT NULL,
    Notes NVARCHAR(500) NULL,
    LoggedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_FoodLogs_Athletes FOREIGN KEY (AthleteId) REFERENCES Athletes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_FoodLogs_Foods FOREIGN KEY (FoodId) REFERENCES Foods(Id),
    CONSTRAINT CK_FoodLogs_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_FoodLogs_MealType CHECK (MealType IN ('breakfast', 'lunch', 'dinner', 'snack'))
);

CREATE INDEX IX_FoodLogs_AthleteId ON FoodLogs(AthleteId);
CREATE INDEX IX_FoodLogs_LogDate ON FoodLogs(LogDate);
CREATE INDEX IX_FoodLogs_MealType ON FoodLogs(MealType);
```

---

## 8. Communication System

### 8.1 Conversations Table
```sql
CREATE TABLE Conversations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Type TINYINT NOT NULL DEFAULT 0, -- 0: Direct, 1: Group
    Title NVARCHAR(200) NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Conversations_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_Conversations_Type CHECK (Type IN (0, 1))
);

CREATE INDEX IX_Conversations_CreatedBy ON Conversations(CreatedBy);
CREATE INDEX IX_Conversations_UpdatedAt ON Conversations(UpdatedAt);
```

### 8.2 ConversationParticipants Table
```sql
CREATE TABLE ConversationParticipants (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ConversationId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Role TINYINT NOT NULL DEFAULT 0, -- 0: Member, 1: Admin
    JoinedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    LeftAt DATETIMEOFFSET NULL,
    LastReadAt DATETIMEOFFSET NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_ConversationParticipants_Conversations FOREIGN KEY (ConversationId) REFERENCES Conversations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ConversationParticipants_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT CK_ConversationParticipants_Role CHECK (Role IN (0, 1)),
    CONSTRAINT UQ_ConversationParticipants_Conversation_User UNIQUE (ConversationId, UserId)
);

CREATE INDEX IX_ConversationParticipants_ConversationId ON ConversationParticipants(ConversationId);
CREATE INDEX IX_ConversationParticipants_UserId ON ConversationParticipants(UserId);
```

### 8.3 Messages Table
```sql
CREATE TABLE Messages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ConversationId UNIQUEIDENTIFIER NOT NULL,
    SenderId UNIQUEIDENTIFIER NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    MessageType TINYINT NOT NULL DEFAULT 0, -- 0: Text, 1: Image, 2: File, 3: Voice, 4: Video, 5: System
    ReplyToMessageId UNIQUEIDENTIFIER NULL,
    IsEdited BIT NOT NULL DEFAULT 0,
    EditedAt DATETIMEOFFSET NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIMEOFFSET NULL,
    SentAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Messages_Conversations FOREIGN KEY (ConversationId) REFERENCES Conversations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Messages_Senders FOREIGN KEY (SenderId) REFERENCES Users(Id),
    CONSTRAINT FK_Messages_ReplyTo FOREIGN KEY (ReplyToMessageId) REFERENCES Messages(Id),
    CONSTRAINT CK_Messages_MessageType CHECK (MessageType BETWEEN 0 AND 5)
);

CREATE INDEX IX_Messages_ConversationId ON Messages(ConversationId);
CREATE INDEX IX_Messages_SenderId ON Messages(SenderId);
CREATE INDEX IX_Messages_SentAt ON Messages(SentAt);
CREATE INDEX IX_Messages_ReplyToMessageId ON Messages(ReplyToMessageId);
```

### 8.4 MessageAttachments Table
```sql
CREATE TABLE MessageAttachments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MessageId UNIQUEIDENTIFIER NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FileUrl NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    MimeType NVARCHAR(100) NOT NULL,
    UploadedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_MessageAttachments_Messages FOREIGN KEY (MessageId) REFERENCES Messages(Id) ON DELETE CASCADE,
    CONSTRAINT CK_MessageAttachments_FileSize CHECK (FileSize > 0)
);

CREATE INDEX IX_MessageAttachments_MessageId ON MessageAttachments(MessageId);
```

---

## 9. Analytics & Reporting

### 9.1 Notifications Table
```sql
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL, -- workout_reminder, message, progress_update, etc.
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(1000) NOT NULL,
    Data NVARCHAR(MAX) NULL, -- JSON data for notification context
    IsRead BIT NOT NULL DEFAULT 0,
    ReadAt DATETIMEOFFSET NULL,
    DeliveryMethod NVARCHAR(20) NOT NULL DEFAULT 'in_app', -- in_app, email, sms, push
    IsDelivered BIT NOT NULL DEFAULT 0,
    DeliveredAt DATETIMEOFFSET NULL,
    ExpiresAt DATETIMEOFFSET NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_Type ON Notifications(Type);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);
CREATE INDEX IX_Notifications_CreatedAt ON Notifications(CreatedAt);
```

### 9.2 UserActivityLogs Table
```sql
CREATE TABLE UserActivityLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    ActivityType NVARCHAR(50) NOT NULL, -- login, logout, workout_start, workout_complete, etc.
    Description NVARCHAR(500) NULL,
    EntityType NVARCHAR(50) NULL, -- Workout, Exercise, etc.
    EntityId UNIQUEIDENTIFIER NULL,
    IpAddress NVARCHAR(45) NULL,
    UserAgent NVARCHAR(500) NULL,
    Metadata NVARCHAR(MAX) NULL, -- JSON metadata
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_UserActivityLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE INDEX IX_UserActivityLogs_UserId ON UserActivityLogs(UserId);
CREATE INDEX IX_UserActivityLogs_ActivityType ON UserActivityLogs(ActivityType);
CREATE INDEX IX_UserActivityLogs_CreatedAt ON UserActivityLogs(CreatedAt);
CREATE INDEX IX_UserActivityLogs_EntityType_EntityId ON UserActivityLogs(EntityType, EntityId);
```

### 9.3 SystemMetrics Table
```sql
CREATE TABLE SystemMetrics (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MetricName NVARCHAR(100) NOT NULL,
    MetricValue DECIMAL(18,6) NOT NULL,
    Unit NVARCHAR(20) NULL,
    Category NVARCHAR(50) NOT NULL, -- performance, usage, error, etc.
    Source NVARCHAR(100) NOT NULL, -- api, database, background_job, etc.
    Metadata NVARCHAR(MAX) NULL, -- JSON metadata
    RecordedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT CK_SystemMetrics_MetricName CHECK (LEN(MetricName) > 0)
);

CREATE INDEX IX_SystemMetrics_MetricName ON SystemMetrics(MetricName);
CREATE INDEX IX_SystemMetrics_Category ON SystemMetrics(Category);
CREATE INDEX IX_SystemMetrics_RecordedAt ON SystemMetrics(RecordedAt);

-- Partitioning by month for performance
ALTER TABLE SystemMetrics 
ADD CONSTRAINT CK_SystemMetrics_RecordedAt_Range 
CHECK (RecordedAt >= '2024-01-01');
```

---

## 10. System Configuration

### 10.1 ApplicationSettings Table
```sql
CREATE TABLE ApplicationSettings (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX) NOT NULL,
    DataType NVARCHAR(20) NOT NULL DEFAULT 'string', -- string, int, decimal, bool, json
    Category NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500) NULL,
    IsEditable BIT NOT NULL DEFAULT 1,
    IsEncrypted BIT NOT NULL DEFAULT 0,
    ValidValues NVARCHAR(MAX) NULL, -- JSON array for validation
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NULL,
    
    CONSTRAINT FK_ApplicationSettings_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_ApplicationSettings_DataType CHECK (
        DataType IN ('string', 'int', 'decimal', 'bool', 'json')
    )
);

CREATE INDEX IX_ApplicationSettings_Category ON ApplicationSettings(Category);

-- Default settings
INSERT INTO ApplicationSettings (SettingKey, SettingValue, DataType, Category, Description) VALUES
('MaxFileUploadSize', '10485760', 'int', 'uploads', 'Maximum file upload size in bytes (10MB)'),
('SessionTimeoutMinutes', '60', 'int', 'authentication', 'User session timeout in minutes'),
('MaxWorkoutsPerDay', '3', 'int', 'business_rules', 'Maximum workouts an athlete can have per day'),
('EnableWorkoutReminders', 'true', 'bool', 'notifications', 'Enable workout reminder notifications'),
('DefaultCurrency', 'USD', 'string', 'localization', 'Default currency code');
```

### 10.2 ErrorLogs Table
```sql
CREATE TABLE ErrorLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL,
    ErrorMessage NVARCHAR(MAX) NOT NULL,
    StackTrace NVARCHAR(MAX) NULL,
    Source NVARCHAR(255) NULL,
    RequestUrl NVARCHAR(2000) NULL,
    HttpMethod NVARCHAR(10) NULL,
    IpAddress NVARCHAR(45) NULL,
    UserAgent NVARCHAR(500) NULL,
    ErrorLevel NVARCHAR(20) NOT NULL DEFAULT 'Error', -- Debug, Info, Warning, Error, Fatal
    Category NVARCHAR(50) NULL,
    AdditionalData NVARCHAR(MAX) NULL, -- JSON
    IsResolved BIT NOT NULL DEFAULT 0,
    ResolvedAt DATETIMEOFFSET NULL,
    ResolvedBy UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIMEOFFSET NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_ErrorLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_ErrorLogs_ResolvedBy FOREIGN KEY (ResolvedBy) REFERENCES Users(Id)
);

CREATE INDEX IX_ErrorLogs_UserId ON ErrorLogs(UserId);
CREATE INDEX IX_ErrorLogs_ErrorLevel ON ErrorLogs(ErrorLevel);
CREATE INDEX IX_ErrorLogs_CreatedAt ON ErrorLogs(CreatedAt);
CREATE INDEX IX_ErrorLogs_IsResolved ON ErrorLogs(IsResolved);
```

---

## Appendix A: Database Maintenance

### A.1 Backup Strategy
```sql
-- Full backup weekly
BACKUP DATABASE WorkoutProjectDB 
TO DISK = 'C:\Backups\WorkoutProjectDB_Full.bak'
WITH COMPRESSION, CHECKSUM;

-- Differential backup daily
BACKUP DATABASE WorkoutProjectDB 
TO DISK = 'C:\Backups\WorkoutProjectDB_Diff.bak'
WITH DIFFERENTIAL, COMPRESSION, CHECKSUM;

-- Transaction log backup every 15 minutes
BACKUP LOG WorkoutProjectDB 
TO DISK = 'C:\Backups\WorkoutProjectDB_Log.trn'
WITH COMPRESSION, CHECKSUM;
```

### A.2 Index Maintenance
```sql
-- Rebuild indexes weekly
ALTER INDEX ALL ON Users REBUILD WITH (ONLINE = ON);
ALTER INDEX ALL ON Workouts REBUILD WITH (ONLINE = ON);
ALTER INDEX ALL ON WorkoutSessions REBUILD WITH (ONLINE = ON);

-- Update statistics daily
UPDATE STATISTICS Users;
UPDATE STATISTICS Workouts;
UPDATE STATISTICS WorkoutSessions;
```

### A.3 Cleanup Procedures
```sql
-- Clean up old error logs (older than 90 days)
DELETE FROM ErrorLogs 
WHERE CreatedAt < DATEADD(DAY, -90, GETUTCDATE());

-- Clean up expired refresh tokens
DELETE FROM RefreshTokens 
WHERE ExpiryDate < GETUTCDATE() OR IsRevoked = 1;

-- Archive old workout sessions (older than 2 years)
-- Move to archive table before deletion
```

---

## Appendix B: Performance Optimization

### B.1 Query Optimization
```sql
-- Covering index for common workout queries
CREATE INDEX IX_Workouts_Covering 
ON Workouts (AthleteId, ScheduledDate, Status) 
INCLUDE (Id, Name, WorkoutType, EstimatedDurationMinutes);

-- Filtered index for active users
CREATE INDEX IX_Users_Active_Filtered 
ON Users (CreatedAt, LastLoginAt) 
WHERE IsActive = 1 AND IsDeleted = 0;
```

### B.2 Partitioning Strategy
```sql
-- Partition WorkoutSessions by month
CREATE PARTITION FUNCTION PF_MonthlyPartition (DATETIMEOFFSET)
AS RANGE RIGHT FOR VALUES 
('2024-01-01', '2024-02-01', '2024-03-01', '2024-04-01');

CREATE PARTITION SCHEME PS_MonthlyPartition
AS PARTITION PF_MonthlyPartition
ALL TO ([PRIMARY]);

-- Apply partitioning to WorkoutSessions
CREATE TABLE WorkoutSessions_Partitioned (
    -- Same structure as WorkoutSessions
) ON PS_MonthlyPartition(StartedAt);
```

---

## Appendix C: Security Considerations

### C.1 Row-Level Security
```sql
-- Enable RLS for multi-tenant data isolation
ALTER TABLE Workouts ENABLE ROW LEVEL SECURITY;

-- Policy for trainers - can only see their own workouts
CREATE SECURITY POLICY TrainerWorkoutsPolicy
ON Workouts
FOR ALL
TO TrainerRole
USING (TrainerId = CAST(SESSION_CONTEXT(N'UserId') AS UNIQUEIDENTIFIER));

-- Policy for athletes - can only see assigned workouts
CREATE SECURITY POLICY AthleteWorkoutsPolicy
ON Workouts
FOR ALL
TO AthleteRole
USING (AthleteId = CAST(SESSION_CONTEXT(N'UserId') AS UNIQUEIDENTIFIER));
```

### C.2 Data Encryption
```sql
-- Encrypt sensitive columns
ALTER TABLE Users
ADD EncryptedSSN VARBINARY(256);

-- Use Always Encrypted for PII data
-- Configure through SSMS or PowerShell
```

### C.3 Audit Configuration
```sql
-- Enable SQL Server audit
CREATE SERVER AUDIT WorkoutProjectAudit
TO FILE (FILEPATH = 'C:\Audits\', MAXSIZE = 100MB, MAX_ROLLOVER_FILES = 10);

ALTER SERVER AUDIT WorkoutProjectAudit WITH (STATE = ON);

-- Create database audit specification
CREATE DATABASE AUDIT SPECIFICATION WorkoutProjectDBAudit
FOR SERVER AUDIT WorkoutProjectAudit
ADD (SELECT, INSERT, UPDATE, DELETE ON Users BY public),
ADD (SELECT, INSERT, UPDATE, DELETE ON Workouts BY public);

ALTER DATABASE AUDIT SPECIFICATION WorkoutProjectDBAudit WITH (STATE = ON);
```
