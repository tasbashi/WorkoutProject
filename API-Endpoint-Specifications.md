# API Endpoint Specifications
## Coach-Athlete Tracking System

---

## Table of Contents
1. [API Overview](#1-api-overview)
2. [Authentication Endpoints](#2-authentication-endpoints)
3. [User Management](#3-user-management)
4. [Workout Management](#4-workout-management)
5. [Exercise Management](#5-exercise-management)
6. [Training Programs](#6-training-programs)
7. [Performance Analytics](#7-performance-analytics)
8. [Nutrition Tracking](#8-nutrition-tracking)
9. [Messaging System](#9-messaging-system)
10. [Notification Management](#10-notification-management)

---

## 1. API Overview

### 1.1 Base URL
```
Development: https://localhost:5001/api/v1
Staging: https://api-staging.workoutproject.com/api/v1
Production: https://api.workoutproject.com/api/v1
```

### 1.2 Common Response Format
```json
{
  "success": true,
  "data": {},
  "message": "Operation completed successfully",
  "errors": [],
  "pagination": {
    "currentPage": 1,
    "pageSize": 10,
    "totalPages": 5,
    "totalItems": 45,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

### 1.3 HTTP Status Codes
- `200 OK` - Successful GET, PUT, PATCH
- `201 Created` - Successful POST
- `204 No Content` - Successful DELETE
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Access denied
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict
- `422 Unprocessable Entity` - Validation errors
- `500 Internal Server Error` - Server error

### 1.4 Error Response Format
```json
{
  "success": false,
  "data": null,
  "message": "Validation failed",
  "errors": [
    {
      "field": "email",
      "message": "Email is required"
    },
    {
      "field": "password",
      "message": "Password must be at least 8 characters"
    }
  ]
}
```

---

## 2. Authentication Endpoints

### 2.1 Register User
```http
POST /auth/register
```

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass123!",
  "confirmPassword": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "username": "johndoe",
  "role": "athlete", // "athlete" | "trainer" | "admin"
  "dateOfBirth": "1990-05-15",
  "gender": "male", // "male" | "female" | "other"
  "phoneNumber": "+1234567890"
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "username": "johndoe",
      "role": "athlete",
      "emailConfirmed": false,
      "createdAt": "2024-01-15T10:30:00Z"
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "550e8400-e29b-41d4-a716-446655440001",
    "expiresIn": 3600
  },
  "message": "Registration successful. Please check your email to verify your account."
}
```

### 2.2 Login User
```http
POST /auth/login
```

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass123!",
  "rememberMe": true
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "username": "johndoe",
      "role": "athlete",
      "profilePictureUrl": "https://storage.example.com/profiles/john-doe.jpg",
      "emailConfirmed": true,
      "lastLoginAt": "2024-01-15T10:30:00Z"
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "550e8400-e29b-41d4-a716-446655440001",
    "expiresIn": 3600
  },
  "message": "Login successful"
}
```

### 2.3 Refresh Token
```http
POST /auth/refresh
```

**Request Body:**
```json
{
  "refreshToken": "550e8400-e29b-41d4-a716-446655440001"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "550e8400-e29b-41d4-a716-446655440002",
    "expiresIn": 3600
  }
}
```

### 2.4 Logout User
```http
POST /auth/logout
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "refreshToken": "550e8400-e29b-41d4-a716-446655440001"
}
```

**Response:** `204 No Content`

### 2.5 Forgot Password
```http
POST /auth/forgot-password
```

**Request Body:**
```json
{
  "email": "john.doe@example.com"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Password reset instructions sent to your email"
}
```

### 2.6 Reset Password
```http
POST /auth/reset-password
```

**Request Body:**
```json
{
  "token": "reset-token-from-email",
  "newPassword": "NewSecurePass123!",
  "confirmPassword": "NewSecurePass123!"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Password reset successful"
}
```

---

## 3. User Management

### 3.1 Get Current User Profile
```http
GET /users/profile
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "username": "johndoe",
    "role": "athlete",
    "dateOfBirth": "1990-05-15",
    "gender": "male",
    "phoneNumber": "+1234567890",
    "profilePictureUrl": "https://storage.example.com/profiles/john-doe.jpg",
    "emailConfirmed": true,
    "twoFactorEnabled": false,
    "createdAt": "2024-01-01T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z",
    "athlete": {
      "id": "550e8400-e29b-41d4-a716-446655440100",
      "height": 180.5,
      "weight": 75.0,
      "activityLevel": "moderate",
      "goals": ["weight_loss", "muscle_gain"],
      "medicalHistory": "No significant medical history",
      "emergencyContact": {
        "name": "Jane Doe",
        "phone": "+1234567891",
        "relationship": "spouse"
      },
      "trainerId": "550e8400-e29b-41d4-a716-446655440200"
    }
  }
}
```

### 3.2 Update User Profile
```http
PUT /users/profile
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1990-05-15",
  "gender": "male",
  "phoneNumber": "+1234567890",
  "athlete": {
    "height": 180.5,
    "weight": 75.0,
    "activityLevel": "moderate",
    "goals": ["weight_loss", "muscle_gain"],
    "medicalHistory": "No significant medical history",
    "emergencyContact": {
      "name": "Jane Doe",
      "phone": "+1234567891",
      "relationship": "spouse"
    }
  }
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    // Updated user profile object
  },
  "message": "Profile updated successfully"
}
```

### 3.3 Upload Profile Picture
```http
POST /users/profile/picture
Authorization: Bearer {accessToken}
Content-Type: multipart/form-data
```

**Request Body:**
```
file: [image file]
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "profilePictureUrl": "https://storage.example.com/profiles/john-doe-new.jpg"
  },
  "message": "Profile picture updated successfully"
}
```

### 3.4 Change Password
```http
PUT /users/password
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!",
  "confirmPassword": "NewPassword123!"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Password changed successfully"
}
```

---

## 4. Workout Management

### 4.1 Get Workouts
```http
GET /workouts?page=1&pageSize=10&type=strength&status=assigned&athleteId={athleteId}&fromDate=2024-01-01&toDate=2024-01-31&search=morning
Authorization: Bearer {accessToken}
```

**Query Parameters:**
- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 10, max: 100) - Items per page
- `type` (string, optional) - Workout type filter
- `status` (string, optional) - Workout status filter
- `athleteId` (guid, optional) - Filter by athlete (trainers only)
- `fromDate` (date, optional) - Start date filter
- `toDate` (date, optional) - End date filter
- `search` (string, optional) - Search in name and description

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "Morning Strength Training",
      "description": "Full body strength workout",
      "type": "strength",
      "status": "assigned",
      "scheduledDate": "2024-01-16T08:00:00Z",
      "estimatedDurationMinutes": 60,
      "createdAt": "2024-01-15T10:30:00Z",
      "trainer": {
        "id": "550e8400-e29b-41d4-a716-446655440200",
        "firstName": "Mike",
        "lastName": "Johnson",
        "profilePictureUrl": "https://storage.example.com/profiles/mike-johnson.jpg"
      },
      "athlete": {
        "id": "550e8400-e29b-41d4-a716-446655440100",
        "firstName": "John",
        "lastName": "Doe"
      },
      "exerciseCount": 5,
      "completionStatus": {
        "isCompleted": false,
        "completionPercentage": 0,
        "lastSessionDate": null
      }
    }
  ],
  "pagination": {
    "currentPage": 1,
    "pageSize": 10,
    "totalPages": 3,
    "totalItems": 25,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

### 4.2 Get Workout by ID
```http
GET /workouts/{workoutId}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Morning Strength Training",
    "description": "Full body strength workout focusing on compound movements",
    "type": "strength",
    "status": "assigned",
    "scheduledDate": "2024-01-16T08:00:00Z",
    "estimatedDurationMinutes": 60,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z",
    "trainer": {
      "id": "550e8400-e29b-41d4-a716-446655440200",
      "firstName": "Mike",
      "lastName": "Johnson",
      "profilePictureUrl": "https://storage.example.com/profiles/mike-johnson.jpg"
    },
    "athlete": {
      "id": "550e8400-e29b-41d4-a716-446655440100",
      "firstName": "John",
      "lastName": "Doe",
      "profilePictureUrl": "https://storage.example.com/profiles/john-doe.jpg"
    },
    "exercises": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440001",
        "exercise": {
          "id": "550e8400-e29b-41d4-a716-446655440301",
          "name": "Barbell Squat",
          "category": "legs",
          "muscleGroups": ["quadriceps", "glutes", "hamstrings"],
          "equipment": "barbell",
          "instructions": "Stand with feet shoulder-width apart...",
          "imageUrl": "https://storage.example.com/exercises/barbell-squat.jpg",
          "videoUrl": "https://storage.example.com/exercises/barbell-squat.mp4"
        },
        "sets": 4,
        "reps": 8,
        "weight": 80.0,
        "restTimeSeconds": 120,
        "notes": "Focus on form over weight",
        "order": 1
      }
    ],
    "sessions": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440400",
        "startedAt": "2024-01-16T08:00:00Z",
        "completedAt": null,
        "durationMinutes": null,
        "status": "in_progress"
      }
    ]
  }
}
```

### 4.3 Create Workout
```http
POST /workouts
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "name": "Morning Strength Training",
  "description": "Full body strength workout",
  "type": "strength",
  "scheduledDate": "2024-01-16T08:00:00Z",
  "estimatedDurationMinutes": 60,
  "athleteId": "550e8400-e29b-41d4-a716-446655440100",
  "exercises": [
    {
      "exerciseId": "550e8400-e29b-41d4-a716-446655440301",
      "sets": 4,
      "reps": 8,
      "weight": 80.0,
      "restTimeSeconds": 120,
      "notes": "Focus on form",
      "order": 1
    },
    {
      "exerciseId": "550e8400-e29b-41d4-a716-446655440302",
      "sets": 3,
      "reps": 10,
      "weight": 60.0,
      "restTimeSeconds": 90,
      "order": 2
    }
  ]
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    // Full workout object as in GET response
  },
  "message": "Workout created successfully"
}
```

### 4.4 Update Workout
```http
PUT /workouts/{workoutId}
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "name": "Updated Morning Strength Training",
  "description": "Updated description",
  "scheduledDate": "2024-01-17T08:00:00Z",
  "estimatedDurationMinutes": 75,
  "exercises": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001", // Existing exercise to update
      "exerciseId": "550e8400-e29b-41d4-a716-446655440301",
      "sets": 5,
      "reps": 8,
      "weight": 85.0,
      "restTimeSeconds": 120,
      "order": 1
    },
    {
      // New exercise (no id provided)
      "exerciseId": "550e8400-e29b-41d4-a716-446655440303",
      "sets": 3,
      "reps": 12,
      "weight": 50.0,
      "restTimeSeconds": 60,
      "order": 2
    }
  ]
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    // Updated workout object
  },
  "message": "Workout updated successfully"
}
```

### 4.5 Delete Workout
```http
DELETE /workouts/{workoutId}
Authorization: Bearer {accessToken}
```

**Response:** `204 No Content`

### 4.6 Assign Workout to Athlete
```http
POST /workouts/{workoutId}/assign
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "athleteId": "550e8400-e29b-41d4-a716-446655440100",
  "scheduledDate": "2024-01-16T08:00:00Z",
  "notes": "Remember to warm up properly"
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Workout assigned to athlete successfully"
}
```

### 4.7 Start Workout Session
```http
POST /workouts/{workoutId}/sessions
Authorization: Bearer {accessToken}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    "sessionId": "550e8400-e29b-41d4-a716-446655440400",
    "workoutId": "550e8400-e29b-41d4-a716-446655440000",
    "athleteId": "550e8400-e29b-41d4-a716-446655440100",
    "startedAt": "2024-01-16T08:00:00Z",
    "status": "in_progress"
  },
  "message": "Workout session started"
}
```

---

## 5. Exercise Management

### 5.1 Get Exercises
```http
GET /exercises?page=1&pageSize=20&category=legs&muscleGroup=quadriceps&equipment=barbell&search=squat
Authorization: Bearer {accessToken}
```

**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 20, max: 100)
- `category` (string, optional) - Exercise category
- `muscleGroup` (string, optional) - Target muscle group
- `equipment` (string, optional) - Required equipment
- `search` (string, optional) - Search in name and instructions

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440301",
      "name": "Barbell Squat",
      "category": "legs",
      "muscleGroups": ["quadriceps", "glutes", "hamstrings"],
      "equipment": "barbell",
      "difficulty": "intermediate",
      "instructions": "Stand with feet shoulder-width apart, hold barbell on upper back...",
      "imageUrl": "https://storage.example.com/exercises/barbell-squat.jpg",
      "videoUrl": "https://storage.example.com/exercises/barbell-squat.mp4",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "pagination": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalItems": 95,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

### 5.2 Get Exercise by ID
```http
GET /exercises/{exerciseId}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440301",
    "name": "Barbell Squat",
    "category": "legs",
    "muscleGroups": ["quadriceps", "glutes", "hamstrings"],
    "equipment": "barbell",
    "difficulty": "intermediate",
    "instructions": "Stand with feet shoulder-width apart...",
    "imageUrl": "https://storage.example.com/exercises/barbell-squat.jpg",
    "videoUrl": "https://storage.example.com/exercises/barbell-squat.mp4",
    "tips": [
      "Keep your chest up and core engaged",
      "Don't let knees cave inward",
      "Full range of motion for best results"
    ],
    "variations": [
      {
        "name": "Front Squat",
        "description": "Hold barbell in front rack position"
      },
      {
        "name": "Goblet Squat",
        "description": "Hold dumbbell at chest level"
      }
    ],
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
}
```

### 5.3 Create Exercise (Trainers/Admins only)
```http
POST /exercises
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "name": "Bulgarian Split Squat",
  "category": "legs",
  "muscleGroups": ["quadriceps", "glutes"],
  "equipment": "bodyweight",
  "difficulty": "intermediate",
  "instructions": "Stand 2-3 feet in front of a bench...",
  "tips": [
    "Keep most weight on front leg",
    "Don't let front knee go past toes"
  ]
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    // Full exercise object
  },
  "message": "Exercise created successfully"
}
```

---

## 6. Training Programs

### 6.1 Get Training Programs
```http
GET /programs?page=1&pageSize=10&type=strength&level=beginner&duration=8
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440500",
      "name": "Beginner Strength Program",
      "description": "8-week strength building program for beginners",
      "type": "strength",
      "level": "beginner",
      "durationWeeks": 8,
      "workoutsPerWeek": 3,
      "createdBy": {
        "id": "550e8400-e29b-41d4-a716-446655440200",
        "firstName": "Mike",
        "lastName": "Johnson"
      },
      "workoutTemplates": [
        {
          "id": "550e8400-e29b-41d4-a716-446655440501",
          "name": "Upper Body Day",
          "dayOfWeek": 1,
          "exerciseCount": 6
        }
      ],
      "enrolledAthletes": 15,
      "rating": 4.8,
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ]
}
```

### 6.2 Enroll in Program
```http
POST /programs/{programId}/enroll
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "startDate": "2024-01-16"
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    "enrollmentId": "550e8400-e29b-41d4-a716-446655440600",
    "programId": "550e8400-e29b-41d4-a716-446655440500",
    "athleteId": "550e8400-e29b-41d4-a716-446655440100",
    "startDate": "2024-01-16",
    "status": "active"
  },
  "message": "Successfully enrolled in program"
}
```

---

## 7. Performance Analytics

### 7.1 Get Performance Dashboard
```http
GET /analytics/dashboard?period=30d&athleteId={athleteId}
Authorization: Bearer {accessToken}
```

**Query Parameters:**
- `period` (string) - Time period: 7d, 30d, 90d, 1y
- `athleteId` (guid, optional) - Specific athlete (trainers only)

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "period": "30d",
    "athlete": {
      "id": "550e8400-e29b-41d4-a716-446655440100",
      "firstName": "John",
      "lastName": "Doe"
    },
    "summary": {
      "totalWorkouts": 12,
      "totalDuration": 720, // minutes
      "averageWorkoutDuration": 60,
      "completionRate": 85.7,
      "totalVolume": 15250.0 // kg
    },
    "progressMetrics": {
      "strengthProgress": [
        {
          "exercise": "Barbell Squat",
          "startWeight": 70.0,
          "currentWeight": 85.0,
          "improvement": 21.4
        }
      ],
      "bodyComposition": [
        {
          "date": "2024-01-01",
          "weight": 76.5,
          "bodyFat": 15.2,
          "muscleMass": 64.8
        }
      ]
    },
    "workoutFrequency": [
      {
        "week": "2024-W01",
        "workouts": 3
      }
    ],
    "topExercises": [
      {
        "exerciseName": "Barbell Squat",
        "totalSets": 48,
        "averageWeight": 82.5,
        "volumeLifted": 3168.0
      }
    ]
  }
}
```

### 7.2 Get Progress Charts
```http
GET /analytics/progress?metric=weight&exercise=barbell-squat&period=90d
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "metric": "weight",
    "exercise": "Barbell Squat",
    "period": "90d",
    "dataPoints": [
      {
        "date": "2024-01-01",
        "value": 70.0,
        "reps": 8,
        "sets": 4,
        "oneRepMax": 87.5
      },
      {
        "date": "2024-01-08",
        "value": 72.5,
        "reps": 8,
        "sets": 4,
        "oneRepMax": 90.6
      }
    ],
    "trend": {
      "direction": "increasing",
      "percentage": 21.4,
      "totalImprovement": 15.0
    }
  }
}
```

### 7.3 Get Personal Records
```http
GET /analytics/personal-records?athleteId={athleteId}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "exercise": {
        "id": "550e8400-e29b-41d4-a716-446655440301",
        "name": "Barbell Squat"
      },
      "oneRepMax": 95.0,
      "maxVolume": 3840.0,
      "longestSet": 15,
      "achievedAt": "2024-01-15T08:30:00Z",
      "previousRecord": 90.0,
      "improvement": 5.6
    }
  ]
}
```

---

## 8. Nutrition Tracking

### 8.1 Get Daily Nutrition
```http
GET /nutrition/daily?date=2024-01-15
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "date": "2024-01-15",
    "goals": {
      "calories": 2200,
      "protein": 165,
      "carbohydrates": 220,
      "fat": 73,
      "water": 3000
    },
    "consumed": {
      "calories": 1850,
      "protein": 140,
      "carbohydrates": 185,
      "fat": 62,
      "water": 2500
    },
    "remaining": {
      "calories": 350,
      "protein": 25,
      "carbohydrates": 35,
      "fat": 11,
      "water": 500
    },
    "meals": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440700",
        "name": "Breakfast",
        "time": "2024-01-15T08:00:00Z",
        "foods": [
          {
            "id": "550e8400-e29b-41d4-a716-446655440701",
            "name": "Oatmeal",
            "quantity": 100,
            "unit": "grams",
            "calories": 389,
            "protein": 16.9,
            "carbohydrates": 66.3,
            "fat": 6.9
          }
        ],
        "totalCalories": 389
      }
    ]
  }
}
```

### 8.2 Log Food Item
```http
POST /nutrition/foods
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "foodId": "550e8400-e29b-41d4-a716-446655440800",
  "quantity": 100,
  "unit": "grams",
  "mealType": "breakfast",
  "consumedAt": "2024-01-15T08:00:00Z"
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440701",
    "food": {
      "id": "550e8400-e29b-41d4-a716-446655440800",
      "name": "Oatmeal",
      "brand": "Generic",
      "servingSize": 100,
      "servingUnit": "grams",
      "caloriesPerServing": 389
    },
    "quantity": 100,
    "unit": "grams",
    "mealType": "breakfast",
    "consumedAt": "2024-01-15T08:00:00Z",
    "nutrition": {
      "calories": 389,
      "protein": 16.9,
      "carbohydrates": 66.3,
      "fat": 6.9
    }
  },
  "message": "Food logged successfully"
}
```

---

## 9. Messaging System

### 9.1 Get Conversations
```http
GET /messages/conversations?page=1&pageSize=20
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440900",
      "participant": {
        "id": "550e8400-e29b-41d4-a716-446655440200",
        "firstName": "Mike",
        "lastName": "Johnson",
        "role": "trainer",
        "profilePictureUrl": "https://storage.example.com/profiles/mike-johnson.jpg",
        "isOnline": true
      },
      "lastMessage": {
        "id": "550e8400-e29b-41d4-a716-446655440901",
        "content": "Great workout today! Keep it up.",
        "sentAt": "2024-01-15T14:30:00Z",
        "senderId": "550e8400-e29b-41d4-a716-446655440200"
      },
      "unreadCount": 2,
      "updatedAt": "2024-01-15T14:30:00Z"
    }
  ]
}
```

### 9.2 Get Conversation Messages
```http
GET /messages/conversations/{conversationId}/messages?page=1&pageSize=50
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440901",
      "conversationId": "550e8400-e29b-41d4-a716-446655440900",
      "senderId": "550e8400-e29b-41d4-a716-446655440200",
      "content": "Great workout today! Keep it up.",
      "messageType": "text",
      "sentAt": "2024-01-15T14:30:00Z",
      "readAt": null,
      "editedAt": null,
      "attachments": []
    }
  ]
}
```

### 9.3 Send Message
```http
POST /messages/conversations/{conversationId}/messages
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "content": "Thank you! Looking forward to tomorrow's session.",
  "messageType": "text",
  "replyToMessageId": "550e8400-e29b-41d4-a716-446655440901"
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440902",
    "conversationId": "550e8400-e29b-41d4-a716-446655440900",
    "senderId": "550e8400-e29b-41d4-a716-446655440100",
    "content": "Thank you! Looking forward to tomorrow's session.",
    "messageType": "text",
    "sentAt": "2024-01-15T14:35:00Z",
    "replyTo": {
      "id": "550e8400-e29b-41d4-a716-446655440901",
      "content": "Great workout today! Keep it up.",
      "senderId": "550e8400-e29b-41d4-a716-446655440200"
    }
  },
  "message": "Message sent successfully"
}
```

---

## 10. Notification Management

### 10.1 Get Notifications
```http
GET /notifications?page=1&pageSize=20&unreadOnly=true
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655441000",
      "type": "workout_reminder",
      "title": "Workout Reminder",
      "message": "You have a workout scheduled in 1 hour: Morning Strength Training",
      "data": {
        "workoutId": "550e8400-e29b-41d4-a716-446655440000",
        "scheduledAt": "2024-01-16T08:00:00Z"
      },
      "isRead": false,
      "createdAt": "2024-01-16T07:00:00Z",
      "readAt": null
    }
  ],
  "pagination": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 2,
    "totalItems": 25,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

### 10.2 Mark Notification as Read
```http
PUT /notifications/{notificationId}/read
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Notification marked as read"
}
```

### 10.3 Mark All Notifications as Read
```http
PUT /notifications/read-all
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "All notifications marked as read"
}
```

### 10.4 Get Notification Settings
```http
GET /notifications/settings
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "data": {
    "workoutReminders": {
      "enabled": true,
      "advanceMinutes": 60,
      "methods": ["push", "email"]
    },
    "messageNotifications": {
      "enabled": true,
      "methods": ["push"]
    },
    "progressUpdates": {
      "enabled": true,
      "frequency": "weekly",
      "methods": ["email"]
    },
    "systemUpdates": {
      "enabled": false,
      "methods": ["email"]
    }
  }
}
```

### 10.5 Update Notification Settings
```http
PUT /notifications/settings
Authorization: Bearer {accessToken}
```

**Request Body:**
```json
{
  "workoutReminders": {
    "enabled": true,
    "advanceMinutes": 30,
    "methods": ["push", "email"]
  },
  "messageNotifications": {
    "enabled": true,
    "methods": ["push"]
  },
  "progressUpdates": {
    "enabled": false,
    "frequency": "monthly",
    "methods": ["email"]
  }
}
```

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Notification settings updated successfully"
}
```

---

## Appendix A: Error Codes

### Common Error Codes
- `AUTH_001` - Invalid credentials
- `AUTH_002` - Token expired
- `AUTH_003` - Insufficient permissions
- `VAL_001` - Validation failed
- `RES_001` - Resource not found
- `RES_002` - Resource already exists
- `SYS_001` - Internal server error
- `SYS_002` - Service unavailable

### Validation Error Example
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    {
      "code": "VAL_001",
      "field": "email",
      "message": "Email is required"
    }
  ]
}
```

---

## Appendix B: Rate Limiting

### Rate Limits by Endpoint Category
- **Authentication**: 5 requests per minute
- **File uploads**: 10 requests per minute
- **General API**: 100 requests per minute
- **Real-time updates**: 1000 requests per minute

### Rate Limit Headers
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1642353600
```

---

## Appendix C: Pagination

### Standard Pagination Parameters
- `page`: Page number (starts from 1)
- `pageSize`: Items per page (default: 10, max: 100)

### Pagination Response
```json
{
  "pagination": {
    "currentPage": 1,
    "pageSize": 10,
    "totalPages": 5,
    "totalItems": 45,
    "hasNext": true,
    "hasPrevious": false
  }
}
```
