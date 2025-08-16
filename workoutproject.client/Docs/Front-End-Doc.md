# Frontend Development Documentation
## Coach-Athlete Tracking System

---

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Technology Stack](#2-technology-stack)
3. [Project Architecture](#3-project-architecture)
4. [Development Environment Setup](#4-development-environment-setup)
5. [Application Structure](#5-application-structure)
6. [Component Architecture](#6-component-architecture)
7. [State Management](#7-state-management)
8. [Routing Strategy](#8-routing-strategy)
9. [API Integration Layer](#9-api-integration-layer)
10. [Authentication & Authorization](#10-authentication--authorization)
11. [UI/UX Components](#11-uiux-components)
12. [Forms & Validation](#12-forms--validation)
13. [Real-time Features](#13-real-time-features)
14. [Performance Optimization](#14-performance-optimization)
15. [Testing Strategy](#15-testing-strategy)
16. [Deployment & CI/CD](#16-deployment--cicd)

---

## 1. Project Overview

The Coach-Athlete Tracking System frontend is a modern, responsive web application built with React and TypeScript. It provides an intuitive interface for coaches to manage their athletes, create training programs, track performance, and communicate effectively. Athletes can view their progress, log workouts, and interact with their coaches through a streamlined dashboard.

### 1.1 Core Objectives
- **Performance**: Sub-2 second page load times
- **Responsiveness**: Mobile-first design approach
- **Accessibility**: WCAG 2.1 AA compliance
- **User Experience**: Intuitive navigation with minimal learning curve
- **Scalability**: Support for 10,000+ concurrent users
- **Offline Capability**: PWA features for core functionality

### 1.2 Target Browsers
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari, Chrome Mobile)

## 2. Technology Stack

### 2.1 Core Technologies
```json
{
  "react": "^18.2.0",
  "typescript": "^5.3.0",
  "react-dom": "^18.2.0",
  "vite": "^5.0.0"
}
```

### 2.2 State Management
```json
{
  "@reduxjs/toolkit": "^2.0.0",
  "react-redux": "^9.0.0",
  "redux-persist": "^6.0.0",
  "reselect": "^5.0.0"
}
```

### 2.3 UI Framework & Styling
```json
{
  "@mui/material": "^5.15.0",
  "@mui/icons-material": "^5.15.0",
  "@mui/lab": "^5.0.0-alpha.150",
  "@mui/x-data-grid": "^6.18.0",
  "@mui/x-date-pickers": "^6.18.0",
  "@emotion/react": "^11.11.0",
  "@emotion/styled": "^11.11.0",
  "styled-components": "^6.1.0"
}
```

### 2.4 Routing & Navigation
```json
{
  "react-router-dom": "^6.20.0",
  "history": "^5.3.0"
}
```

### 2.5 HTTP & WebSocket
```json
{
  "axios": "^1.6.0",
  "socket.io-client": "^4.6.0",
  "@microsoft/signalr": "^8.0.0"
}
```

### 2.6 Forms & Validation
```json
{
  "react-hook-form": "^7.48.0",
  "yup": "^1.3.0",
  "@hookform/resolvers": "^3.3.0"
}
```

### 2.7 Data Visualization
```json
{
  "recharts": "^2.10.0",
  "chart.js": "^4.4.0",
  "react-chartjs-2": "^5.2.0",
  "d3": "^7.8.0"
}
```

### 2.8 Utilities
```json
{
  "date-fns": "^3.0.0",
  "lodash": "^4.17.21",
  "uuid": "^9.0.0",
  "react-intersection-observer": "^9.5.0",
  "react-virtual": "^2.10.0",
  "react-dropzone": "^14.2.0"
}
```

### 2.9 Development Tools
```json
{
  "@types/react": "^18.2.0",
  "@types/react-dom": "^18.2.0",
  "@types/node": "^20.10.0",
  "@typescript-eslint/eslint-plugin": "^6.15.0",
  "@typescript-eslint/parser": "^6.15.0",
  "eslint": "^8.56.0",
  "eslint-plugin-react": "^7.33.0",
  "eslint-plugin-react-hooks": "^4.6.0",
  "prettier": "^3.1.0",
  "husky": "^8.0.0",
  "lint-staged": "^15.2.0",
  "@vitejs/plugin-react": "^4.2.0"
}
```

### 2.10 Testing
```json
{
  "@testing-library/react": "^14.1.0",
  "@testing-library/jest-dom": "^6.1.0",
  "@testing-library/user-event": "^14.5.0",
  "vitest": "^1.1.0",
  "@vitest/ui": "^1.1.0",
  "cypress": "^13.6.0",
  "msw": "^2.0.0"
}
```

## 3. Project Architecture

### 3.1 Architecture Pattern
The frontend follows a **Feature-Sliced Design** architecture combined with **Atomic Design** principles for component organization.

```
src/
├── app/                    # Application initialization
│   ├── providers/         # Context providers
│   ├── store/            # Redux store configuration
│   └── styles/           # Global styles
├── entities/              # Business entities
│   ├── user/
│   ├── workout/
│   ├── exercise/
│   └── nutrition/
├── features/              # Feature-specific logic
│   ├── auth/
│   ├── training/
│   ├── analytics/
│   └── messaging/
├── pages/                 # Route pages
│   ├── dashboard/
│   ├── workouts/
│   ├── athletes/
│   └── settings/
├── shared/                # Shared resources
│   ├── api/
│   ├── components/
│   ├── hooks/
│   ├── lib/
│   └── utils/
└── widgets/               # Complex UI blocks
    ├── header/
    ├── sidebar/
    └── modals/
```

### 3.2 Data Flow Architecture
```
User Action → Component → Action Creator → API Call → Redux Store → Component Update
                            ↓
                    WebSocket Event → Store Update → Component Update
```

## 4. Development Environment Setup

### 4.1 Prerequisites
```bash
# Node.js version
node --version  # Should be >= 18.0.0
npm --version   # Should be >= 9.0.0
```

### 4.2 Initial Setup
```bash
# Clone repository
git clone https://github.com/your-org/coach-athlete-frontend.git
cd coach-athlete-frontend

# Install dependencies
npm install

# Setup environment variables
cp .env.example .env.local

# Run development server
npm run dev
```

### 4.3 Environment Variables
```env
# .env.local
VITE_API_BASE_URL=http://localhost:5000/api
VITE_WEBSOCKET_URL=ws://localhost:5000/hub
VITE_STORAGE_URL=http://localhost:5000/storage
VITE_APP_ENV=development
VITE_SENTRY_DSN=your-sentry-dsn
VITE_GA_TRACKING_ID=your-ga-id
VITE_MAPBOX_TOKEN=your-mapbox-token
VITE_STRIPE_PUBLIC_KEY=your-stripe-key
```

### 4.4 VS Code Configuration
```json
// .vscode/settings.json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "typescript.tsdk": "node_modules/typescript/lib"
}
```

## 5. Application Structure

### 5.1 Entry Point
```typescript
// src/main.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { store } from './app/store';
import { theme } from './app/styles/theme';
import App from './App';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 60 * 1000,
      cacheTime: 5 * 60 * 1000,
      retry: 3,
      refetchOnWindowFocus: false,
    },
  },
});

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <BrowserRouter>
          <ThemeProvider theme={theme}>
            <App />
          </ThemeProvider>
        </BrowserRouter>
      </QueryClientProvider>
    </Provider>
  </React.StrictMode>
);
```

### 5.2 App Component Structure
```typescript
// src/App.tsx
import { Suspense, lazy } from 'react';
import { Routes, Route } from 'react-router-dom';
import { AuthProvider } from './features/auth/AuthProvider';
import { WebSocketProvider } from './shared/providers/WebSocketProvider';
import { NotificationProvider } from './shared/providers/NotificationProvider';
import { LoadingScreen } from './shared/components/LoadingScreen';
import { ErrorBoundary } from './shared/components/ErrorBoundary';
import { PrivateRoute } from './features/auth/PrivateRoute';

const Dashboard = lazy(() => import('./pages/dashboard'));
const Workouts = lazy(() => import('./pages/workouts'));
const Athletes = lazy(() => import('./pages/athletes'));

function App() {
  return (
    <ErrorBoundary>
      <AuthProvider>
        <WebSocketProvider>
          <NotificationProvider>
            <Suspense fallback={<LoadingScreen />}>
              <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route element={<PrivateRoute />}>
                  <Route path="/" element={<Dashboard />} />
                  <Route path="/workouts/*" element={<Workouts />} />
                  <Route path="/athletes/*" element={<Athletes />} />
                </Route>
              </Routes>
            </Suspense>
          </NotificationProvider>
        </WebSocketProvider>
      </AuthProvider>
    </ErrorBoundary>
  );
}
```

## 6. Component Architecture

### 6.1 Component Categories

#### Atomic Components
```typescript
// src/shared/components/atoms/Button/Button.tsx
import { ButtonHTMLAttributes, FC } from 'react';
import { styled } from '@mui/material/styles';
import MuiButton, { ButtonProps as MuiButtonProps } from '@mui/material/Button';

interface ButtonProps extends MuiButtonProps {
  loading?: boolean;
  icon?: React.ReactNode;
}

export const Button: FC<ButtonProps> = ({
  loading,
  children,
  disabled,
  icon,
  ...props
}) => {
  return (
    <StyledButton
      disabled={disabled || loading}
      startIcon={icon}
      {...props}
    >
      {loading ? <CircularProgress size={20} /> : children}
    </StyledButton>
  );
};

const StyledButton = styled(MuiButton)(({ theme }) => ({
  textTransform: 'none',
  borderRadius: theme.spacing(1),
  padding: theme.spacing(1, 3),
}));
```

#### Molecule Components
```typescript
// src/shared/components/molecules/SearchBar/SearchBar.tsx
import { useState, useCallback } from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import { Search, Clear } from '@mui/icons-material';
import { useDebounce } from '../../../hooks/useDebounce';

interface SearchBarProps {
  onSearch: (query: string) => void;
  placeholder?: string;
  delay?: number;
}

export const SearchBar: FC<SearchBarProps> = ({
  onSearch,
  placeholder = 'Search...',
  delay = 300,
}) => {
  const [value, setValue] = useState('');
  const debouncedValue = useDebounce(value, delay);

  useEffect(() => {
    onSearch(debouncedValue);
  }, [debouncedValue, onSearch]);

  const handleClear = useCallback(() => {
    setValue('');
    onSearch('');
  }, [onSearch]);

  return (
    <TextField
      fullWidth
      value={value}
      onChange={(e) => setValue(e.target.value)}
      placeholder={placeholder}
      InputProps={{
        startAdornment: (
          <InputAdornment position="start">
            <Search />
          </InputAdornment>
        ),
        endAdornment: value && (
          <InputAdornment position="end">
            <IconButton onClick={handleClear} edge="end">
              <Clear />
            </IconButton>
          </InputAdornment>
        ),
      }}
    />
  );
};
```

#### Organism Components
```typescript
// src/widgets/WorkoutCard/WorkoutCard.tsx
import { Card, CardContent, CardActions, Typography, Chip } from '@mui/material';
import { format } from 'date-fns';
import { Workout } from '../../entities/workout/types';
import { Button } from '../../shared/components/atoms/Button';

interface WorkoutCardProps {
  workout: Workout;
  onView: (id: string) => void;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
}

export const WorkoutCard: FC<WorkoutCardProps> = ({
  workout,
  onView,
  onEdit,
  onDelete,
}) => {
  return (
    <Card elevation={2}>
      <CardContent>
        <Typography variant="h6" gutterBottom>
          {workout.name}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {format(new Date(workout.date), 'PPP')}
        </Typography>
        <Box mt={2}>
          {workout.exercises.map((exercise) => (
            <Chip
              key={exercise.id}
              label={exercise.name}
              size="small"
              sx={{ mr: 1, mb: 1 }}
            />
          ))}
        </Box>
      </CardContent>
      <CardActions>
        <Button size="small" onClick={() => onView(workout.id)}>
          View
        </Button>
        <Button size="small" onClick={() => onEdit(workout.id)}>
          Edit
        </Button>
        <Button
          size="small"
          color="error"
          onClick={() => onDelete(workout.id)}
        >
          Delete
        </Button>
      </CardActions>
    </Card>
  );
};
```

### 6.2 Component Best Practices

#### Props Interface Pattern
```typescript
// Always define explicit interfaces for props
interface ComponentProps {
  // Required props
  id: string;
  name: string;
  
  // Optional props with defaults
  variant?: 'primary' | 'secondary';
  size?: 'small' | 'medium' | 'large';
  
  // Event handlers
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  onChange?: (value: string) => void;
  
  // Children and render props
  children?: React.ReactNode;
  renderItem?: (item: Item) => React.ReactElement;
}
```

#### Custom Hooks Pattern
```typescript
// src/features/workouts/hooks/useWorkout.ts
export const useWorkout = (workoutId: string) => {
  const dispatch = useAppDispatch();
  const workout = useAppSelector((state) => selectWorkoutById(state, workoutId));
  const loading = useAppSelector(selectWorkoutLoading);
  const error = useAppSelector(selectWorkoutError);

  const updateWorkout = useCallback(
    (data: Partial<Workout>) => {
      dispatch(updateWorkoutAction({ id: workoutId, data }));
    },
    [dispatch, workoutId]
  );

  const deleteWorkout = useCallback(() => {
    dispatch(deleteWorkoutAction(workoutId));
  }, [dispatch, workoutId]);

  return {
    workout,
    loading,
    error,
    updateWorkout,
    deleteWorkout,
  };
};
```

## 7. State Management

### 7.1 Redux Store Structure
```typescript
// src/app/store/index.ts
import { configureStore } from '@reduxjs/toolkit';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import { rootReducer } from './rootReducer';
import { apiMiddleware } from './middleware/api';
import { websocketMiddleware } from './middleware/websocket';

const persistConfig = {
  key: 'root',
  storage,
  whitelist: ['auth', 'user', 'preferences'],
  blacklist: ['ui', 'temp'],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

export const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: ['persist/PERSIST', 'persist/REHYDRATE'],
      },
    })
      .concat(apiMiddleware)
      .concat(websocketMiddleware),
  devTools: process.env.NODE_ENV !== 'production',
});

export const persistor = persistStore(store);
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
```

### 7.2 Slice Pattern
```typescript
// src/features/workouts/store/workoutSlice.ts
import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { workoutApi } from '../api';
import { Workout, WorkoutFilters } from '../types';

interface WorkoutState {
  items: Workout[];
  selectedId: string | null;
  loading: boolean;
  error: string | null;
  filters: WorkoutFilters;
  pagination: {
    page: number;
    pageSize: number;
    total: number;
  };
}

const initialState: WorkoutState = {
  items: [],
  selectedId: null,
  loading: false,
  error: null,
  filters: {},
  pagination: {
    page: 1,
    pageSize: 10,
    total: 0,
  },
};

// Async thunks
export const fetchWorkouts = createAsyncThunk(
  'workouts/fetchWorkouts',
  async (params: { filters?: WorkoutFilters; page?: number }) => {
    const response = await workoutApi.getWorkouts(params);
    return response.data;
  }
);

export const createWorkout = createAsyncThunk(
  'workouts/createWorkout',
  async (data: CreateWorkoutDto) => {
    const response = await workoutApi.createWorkout(data);
    return response.data;
  }
);

// Slice
const workoutSlice = createSlice({
  name: 'workouts',
  initialState,
  reducers: {
    setSelectedWorkout: (state, action: PayloadAction<string | null>) => {
      state.selectedId = action.payload;
    },
    setFilters: (state, action: PayloadAction<WorkoutFilters>) => {
      state.filters = action.payload;
      state.pagination.page = 1;
    },
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // Fetch workouts
      .addCase(fetchWorkouts.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchWorkouts.fulfilled, (state, action) => {
        state.loading = false;
        state.items = action.payload.items;
        state.pagination.total = action.payload.total;
      })
      .addCase(fetchWorkouts.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch workouts';
      })
      // Create workout
      .addCase(createWorkout.fulfilled, (state, action) => {
        state.items.unshift(action.payload);
        state.pagination.total += 1;
      });
  },
});

export const { setSelectedWorkout, setFilters, clearError } = workoutSlice.actions;
export default workoutSlice.reducer;

// Selectors
export const selectAllWorkouts = (state: RootState) => state.workouts.items;
export const selectWorkoutById = (state: RootState, id: string) =>
  state.workouts.items.find((w) => w.id === id);
export const selectWorkoutLoading = (state: RootState) => state.workouts.loading;
```

### 7.3 RTK Query Integration
```typescript
// src/features/api/apiSlice.ts
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { RootState } from '../../app/store';

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_API_BASE_URL,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.token;
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      return headers;
    },
  }),
  tagTypes: ['Workout', 'Exercise', 'Athlete', 'Program'],
  endpoints: (builder) => ({
    getWorkouts: builder.query<Workout[], WorkoutFilters>({
      query: (filters) => ({
        url: '/workouts',
        params: filters,
      }),
      providesTags: ['Workout'],
    }),
    getWorkoutById: builder.query<Workout, string>({
      query: (id) => `/workouts/${id}`,
      providesTags: (result, error, id) => [{ type: 'Workout', id }],
    }),
    createWorkout: builder.mutation<Workout, CreateWorkoutDto>({
      query: (data) => ({
        url: '/workouts',
        method: 'POST',
        body: data,
      }),
      invalidatesTags: ['Workout'],
    }),
    updateWorkout: builder.mutation<Workout, UpdateWorkoutDto>({
      query: ({ id, ...data }) => ({
        url: `/workouts/${id}`,
        method: 'PUT',
        body: data,
      }),
      invalidatesTags: (result, error, { id }) => [{ type: 'Workout', id }],
    }),
  }),
});

export const {
  useGetWorkoutsQuery,
  useGetWorkoutByIdQuery,
  useCreateWorkoutMutation,
  useUpdateWorkoutMutation,
} = apiSlice;
```

## 8. Routing Strategy

### 8.1 Route Configuration
```typescript
// src/app/routes/routeConfig.tsx
import { RouteObject } from 'react-router-dom';
import { lazy } from 'react';
import { PrivateRoute } from '../../features/auth/PrivateRoute';
import { RoleGuard } from '../../features/auth/RoleGuard';
import { Layout } from '../../widgets/Layout';

// Lazy load pages
const Dashboard = lazy(() => import('../../pages/Dashboard'));
const WorkoutList = lazy(() => import('../../pages/workouts/WorkoutList'));
const WorkoutDetail = lazy(() => import('../../pages/workouts/WorkoutDetail'));
const WorkoutCreate = lazy(() => import('../../pages/workouts/WorkoutCreate'));
const AthleteList = lazy(() => import('../../pages/athletes/AthleteList'));
const AthleteProfile = lazy(() => import('../../pages/athletes/AthleteProfile'));

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <PrivateRoute />,
    children: [
      {
        element: <Layout />,
        children: [
          {
            index: true,
            element: <Dashboard />,
          },
          {
            path: 'workouts',
            children: [
              {
                index: true,
                element: <WorkoutList />,
              },
              {
                path: 'new',
                element: (
                  <RoleGuard roles={['trainer', 'admin']}>
                    <WorkoutCreate />
                  </RoleGuard>
                ),
              },
              {
                path: ':workoutId',
                element: <WorkoutDetail />,
              },
            ],
          },
          {
            path: 'athletes',
            element: (
              <RoleGuard roles={['trainer', 'admin']}>
                <Outlet />
              </RoleGuard>
            ),
            children: [
              {
                index: true,
                element: <AthleteList />,
              },
              {
                path: ':athleteId',
                element: <AthleteProfile />,
              },
            ],
          },
        ],
      },
    ],
  },
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/register',
    element: <Register />,
  },
  {
    path: '*',
    element: <NotFound />,
  },
];
```

### 8.2 Navigation Guards
```typescript
// src/features/auth/PrivateRoute.tsx
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from './hooks/useAuth';
import { LoadingScreen } from '../../shared/components/LoadingScreen';

export const PrivateRoute = () => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <LoadingScreen />;
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};

// src/features/auth/RoleGuard.tsx
interface RoleGuardProps {
  roles: string[];
  children: React.ReactNode;
}

export const RoleGuard: FC<RoleGuardProps> = ({ roles, children }) => {
  const { user } = useAuth();

  if (!user || !roles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
};
```

## 9. API Integration Layer

### 9.1 Axios Configuration
```typescript
// src/shared/api/client.ts
import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';
import { store } from '../../app/store';
import { refreshToken, logout } from '../../features/auth/authSlice';

class ApiClient {
  private client: AxiosInstance;
  private isRefreshing = false;
  private failedQueue: Array<{
    resolve: (value?: any) => void;
    reject: (reason?: any) => void;
  }> = [];

  constructor() {
    this.client = axios.create({
      baseURL: import.meta.env.VITE_API_BASE_URL,
      timeout: 30000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.setupInterceptors();
  }

  private setupInterceptors() {
    // Request interceptor
    this.client.interceptors.request.use(
      (config) => {
        const state = store.getState();
        const token = state.auth.accessToken;

        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
      },
      (error) => Promise.reject(error)
    );

    // Response interceptor
    this.client.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
          if (this.isRefreshing) {
            return new Promise((resolve, reject) => {
              this.failedQueue.push({ resolve, reject });
            })
              .then((token) => {
                originalRequest.headers.Authorization = `Bearer ${token}`;
                return this.client(originalRequest);
              })
              .catch((err) => Promise.reject(err));
          }

          originalRequest._retry = true;
          this.isRefreshing = true;

          try {
            const result = await store.dispatch(refreshToken()).unwrap();
            this.processQueue(null, result.accessToken);
            originalRequest.headers.Authorization = `Bearer ${result.accessToken}`;
            return this.client(originalRequest);
          } catch (refreshError) {
            this.processQueue(refreshError, null);
            store.dispatch(logout());
            window.location.href = '/login';
            return Promise.reject(refreshError);
          } finally {
            this.isRefreshing = false;
          }
        }

        return Promise.reject(error);
      }
    );
  }

  private processQueue(error: any, token: string | null = null) {
    this.failedQueue.forEach((prom) => {
      if (error) {
        prom.reject(error);
      } else {
        prom.resolve(token);
      }
    });

    this.failedQueue = [];
  }

  // HTTP methods
  async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.get<T>(url, config);
    return response.data;
  }

  async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.post<T>(url, data, config);
    return response.data;
  }

  async put<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.put<T>(url, data, config);
    return response.data;
  }

  async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.delete<T>(url, config);
    return response.data;
  }

  async patch<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.patch<T>(url, data, config);
    return response.data;
  }
}

export const apiClient = new ApiClient();
```

### 9.2 API Service Layer
```typescript
// src/features/workouts/api/workoutApi.ts
import { apiClient } from '../../../shared/api/client';
import { 
  Workout, 
  CreateWorkoutDto, 
  UpdateWorkoutDto, 
  WorkoutFilters,
  PaginatedResponse 
} from '../types';

export class WorkoutApi {
  private readonly basePath = '/workouts';

  async getWorkouts(params?: WorkoutFilters): Promise<PaginatedResponse<Workout>> {
    return apiClient.get(this.basePath, { params });
  }

  async getWorkoutById(id: string): Promise<Workout> {
    return apiClient.get(`${this.basePath}/${id}`);
  }

  async createWorkout(data: CreateWorkoutDto): Promise<Workout> {
    return apiClient.post(this.basePath, data);
  }

  async updateWorkout(id: string, data: UpdateWorkoutDto): Promise<Workout> {
    return apiClient.put(`${this.basePath}/${id}`, data);
  }

  async deleteWorkout(id: string): Promise<void> {
    return apiClient.delete(`${this.basePath}/${id}`);
  }

  async duplicateWorkout(id: string): Promise<Workout> {
    return apiClient.post(`${this.basePath}/${id}/duplicate`);
  }

  async assignToAthlete(workoutId: string, athleteId: string): Promise<void> {
    return apiClient.post(`${this.basePath}/${workoutId}/assign`, { athleteId });
  }

  async logWorkout(workoutId: string, data: LogWorkoutDto): Promise<WorkoutLog> {
    return apiClient.post(`${this.basePath}/${workoutId}/log`, data);
  }
}

export const workoutApi = new WorkoutApi();
```

## 10. Authentication & Authorization

### 10.1 Auth Context
```typescript
// src/features/auth/AuthContext.tsx
import { createContext, useContext, useEffect, useState } from 'react';
import { User } from '../../entities/user/types';
import { authApi } from './api/authApi';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: LoginCredentials) => Promise<void>;
  logout: () => Promise<void>;
  register: (data: RegisterData) => Promise<void>;
  updateProfile: (data: Partial<User>) => Promise<void>;
  refreshToken: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const initAuth = async () => {
      const token = localStorage.getItem('accessToken');
      if (token) {
        try {
          const userData = await authApi.getCurrentUser();
          setUser(userData);
        } catch (error) {
          localStorage.removeItem('accessToken');
          localStorage.removeItem('refreshToken');
        }
      }
      setIsLoading(false);
    };

    initAuth();
  }, []);

  const login = async (credentials: LoginCredentials) => {
    const response = await authApi.login(credentials);
    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    setUser(response.user);
  };

  const logout = async () => {
    await authApi.logout();
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    setUser(null);
  };

  const value = {
    user,
    isAuthenticated: !!user,
    isLoading,
    login,
    logout,
    register,
    updateProfile,
    refreshToken,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
```

### 10.2 JWT Token Management
```typescript
// src/features/auth/utils/tokenManager.ts
import jwtDecode from 'jwt-decode';

interface TokenPayload {
  sub: string;
  email: string;
  role: string;
  exp: number;
  iat: number;
}

export class TokenManager {
  private static readonly ACCESS_TOKEN_KEY = 'accessToken';
  private static readonly REFRESH_TOKEN_KEY = 'refreshToken';

  static setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  static getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  static getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  static clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  static isTokenExpired(token: string): boolean {
    try {
      const decoded = jwtDecode<TokenPayload>(token);
      const currentTime = Date.now() / 1000;
      return decoded.exp < currentTime;
    } catch {
      return true;
    }
  }

  static getTokenPayload(token: string): TokenPayload | null {
    try {
      return jwtDecode<TokenPayload>(token);
    } catch {
      return null;
    }
  }

  static shouldRefreshToken(): boolean {
    const accessToken = this.getAccessToken();
    if (!accessToken) return false;

    const payload = this.getTokenPayload(accessToken);
    if (!payload) return false;

    const currentTime = Date.now() / 1000;
    const timeUntilExpiry = payload.exp - currentTime;

    // Refresh if token expires in less than 5 minutes
    return timeUntilExpiry < 300;
  }
}
```

## 11. UI/UX Components

### 11.1 Design System Implementation
```typescript
// src/app/styles/theme.ts
import { createTheme } from '@mui/material/styles';

export const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
      light: '#42a5f5',
      dark: '#1565c0',
      contrastText: '#fff',
    },
    secondary: {
      main: '#dc004e',
      light: '#f50057',
      dark: '#c51162',
      contrastText: '#fff',
    },
    error: {
      main: '#f44336',
    },
    warning: {
      main: '#ff9800',
    },
    info: {
      main: '#2196f3',
    },
    success: {
      main: '#4caf50',
    },
    background: {
      default: '#f5f5f5',
      paper: '#ffffff',
    },
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    h1: {
      fontSize: '2.5rem',
      fontWeight: 600,
    },
    h2: {
      fontSize: '2rem',
      fontWeight: 600,
    },
    h3: {
      fontSize: '1.75rem',
      fontWeight: 600,
    },
    h4: {
      fontSize: '1.5rem',
      fontWeight: 500,
    },
    h5: {
      fontSize: '1.25rem',
      fontWeight: 500,
    },
    h6: {
      fontSize: '1rem',
      fontWeight: 500,
    },
    button: {
      textTransform: 'none',
    },
  },
  shape: {
    borderRadius: 8,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          padding: '8px 16px',
          fontSize: '0.875rem',
          fontWeight: 500,
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
        },
      },
    },
    MuiTextField: {
      defaultProps: {
        variant: 'outlined',
      },
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: 8,
          },
        },
      },
    },
  },
});
```

### 11.2 Responsive Layout
```typescript
// src/widgets/Layout/Layout.tsx
import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { 
  Box, 
  AppBar, 
  Toolbar, 
  Drawer, 
  IconButton,
  useTheme,
  useMediaQuery 
} from '@mui/material';
import { Menu as MenuIcon } from '@mui/icons-material';
import { Sidebar } from '../Sidebar';
import { Header } from '../Header';

export const Layout = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const isTablet = useMediaQuery(theme.breakpoints.between('md', 'lg'));

  const drawerWidth = isMobile ? '100%' : isTablet ? 240 : 280;

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
      <AppBar
        position="fixed"
        sx={{
          zIndex: theme.zIndex.drawer + 1,
          backgroundColor: 'background.paper',
          color: 'text.primary',
          boxShadow: 1,
        }}
      >
        <Toolbar>
          {isMobile && (
            <IconButton
              color="inherit"
              edge="start"
              onClick={() => setSidebarOpen(!sidebarOpen)}
              sx={{ mr: 2 }}
            >
              <MenuIcon />
            </IconButton>
          )}
          <Header />
        </Toolbar>
      </AppBar>

      <Drawer
        variant={isMobile ? 'temporary' : 'permanent'}
        open={isMobile ? sidebarOpen : true}
        onClose={() => setSidebarOpen(false)}
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
            mt: isMobile ? 0 : 8,
          },
        }}
      >
        <Sidebar onClose={() => setSidebarOpen(false)} />
      </Drawer>

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          mt: 8,
          ml: isMobile ? 0 : `${drawerWidth}px`,
        }}
      >
        <Outlet />
      </Box>
    </Box>
  );
};
```

## 12. Forms & Validation

### 12.1 Form Hook Implementation
```typescript
// src/features/workouts/components/WorkoutForm/WorkoutForm.tsx
import { useForm, Controller, useFieldArray } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { 
  TextField, 
  Select, 
  MenuItem, 
  FormControl, 
  FormLabel,
  Button,
  IconButton 
} from '@mui/material';
import { Add, Delete } from '@mui/icons-material';

const workoutSchema = yup.object({
  name: yup.string().required('Workout name is required').min(3).max(100),
  description: yup.string().max(500),
  date: yup.date().required('Date is required'),
  type: yup.string().oneOf(['strength', 'cardio', 'flexibility', 'mixed']).required(),
  duration: yup.number().positive().required('Duration is required'),
  exercises: yup.array().of(
    yup.object({
      exerciseId: yup.string().required('Exercise is required'),
      sets: yup.number().positive().required('Sets are required'),
      reps: yup.number().positive().required('Reps are required'),
      weight: yup.number().positive().nullable(),
      restTime: yup.number().positive().nullable(),
      notes: yup.string().max(200),
    })
  ).min(1, 'At least one exercise is required'),
});

type WorkoutFormData = yup.InferType<typeof workoutSchema>;

interface WorkoutFormProps {
  initialData?: Partial<WorkoutFormData>;
  onSubmit: (data: WorkoutFormData) => Promise<void>;
  onCancel: () => void;
}

export const WorkoutForm: FC<WorkoutFormProps> = ({ 
  initialData, 
  onSubmit, 
  onCancel 
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    watch,
  } = useForm<WorkoutFormData>({
    resolver: yupResolver(workoutSchema),
    defaultValues: initialData || {
      name: '',
      description: '',
      date: new Date(),
      type: 'strength',
      duration: 60,
      exercises: [{ exerciseId: '', sets: 3, reps: 10 }],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'exercises',
  });

  const workoutType = watch('type');

  const onFormSubmit = async (data: WorkoutFormData) => {
    try {
      await onSubmit(data);
    } catch (error) {
      console.error('Form submission error:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit(onFormSubmit)}>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Controller
            name="name"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                label="Workout Name"
                fullWidth
                error={!!errors.name}
                helperText={errors.name?.message}
              />
            )}
          />
        </Grid>

        <Grid item xs={12} md={6}>
          <Controller
            name="type"
            control={control}
            render={({ field }) => (
              <FormControl fullWidth error={!!errors.type}>
                <FormLabel>Workout Type</FormLabel>
                <Select {...field}>
                  <MenuItem value="strength">Strength</MenuItem>
                  <MenuItem value="cardio">Cardio</MenuItem>
                  <MenuItem value="flexibility">Flexibility</MenuItem>
                  <MenuItem value="mixed">Mixed</MenuItem>
                </Select>
              </FormControl>
            )}
          />
        </Grid>

        <Grid item xs={12}>
          <Typography variant="h6" gutterBottom>
            Exercises
          </Typography>
          {fields.map((field, index) => (
            <Box key={field.id} sx={{ mb: 2, p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
              <Grid container spacing={2} alignItems="center">
                <Grid item xs={12} md={3}>
                  <Controller
                    name={`exercises.${index}.exerciseId`}
                    control={control}
                    render={({ field }) => (
                      <ExerciseSelect
                        {...field}
                        error={!!errors.exercises?.[index]?.exerciseId}
                        helperText={errors.exercises?.[index]?.exerciseId?.message}
                      />
                    )}
                  />
                </Grid>

                <Grid item xs={6} md={2}>
                  <Controller
                    name={`exercises.${index}.sets`}
                    control={control}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        type="number"
                        label="Sets"
                        fullWidth
                        error={!!errors.exercises?.[index]?.sets}
                      />
                    )}
                  />
                </Grid>

                <Grid item xs={6} md={2}>
                  <Controller
                    name={`exercises.${index}.reps`}
                    control={control}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        type="number"
                        label="Reps"
                        fullWidth
                        error={!!errors.exercises?.[index]?.reps}
                      />
                    )}
                  />
                </Grid>

                {workoutType === 'strength' && (
                  <Grid item xs={6} md={2}>
                    <Controller
                      name={`exercises.${index}.weight`}
                      control={control}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          type="number"
                          label="Weight (kg)"
                          fullWidth
                        />
                      )}
                    />
                  </Grid>
                )}

                <Grid item xs={6} md={2}>
                  <Controller
                    name={`exercises.${index}.restTime`}
                    control={control}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        type="number"
                        label="Rest (sec)"
                        fullWidth
                      />
                    )}
                  />
                </Grid>

                <Grid item xs={12} md={1}>
                  <IconButton
                    color="error"
                    onClick={() => remove(index)}
                    disabled={fields.length === 1}
                  >
                    <Delete />
                  </IconButton>
                </Grid>
              </Grid>
            </Box>
          ))}

          <Button
            startIcon={<Add />}
            onClick={() => append({ exerciseId: '', sets: 3, reps: 10 })}
            variant="outlined"
            sx={{ mt: 2 }}
          >
            Add Exercise
          </Button>
        </Grid>

        <Grid item xs={12}>
          <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={onCancel} variant="outlined">
              Cancel
            </Button>
            <Button
              type="submit"
              variant="contained"
              disabled={isSubmitting}
            >
              {isSubmitting ? 'Saving...' : 'Save Workout'}
            </Button>
          </Box>
        </Grid>
      </Grid>
    </form>
  );
};
```

## 13. Real-time Features

### 13.1 WebSocket Integration
```typescript
// src/shared/providers/WebSocketProvider.tsx
import { createContext, useContext, useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useAuth } from '../../features/auth/hooks/useAuth';

interface WebSocketContextType {
  connection: HubConnection | null;
  isConnected: boolean;
  subscribe: (event: string, handler: (...args: any[]) => void) => void;
  unsubscribe: (event: string, handler: (...args: any[]) => void) => void;
  send: (event: string, ...args: any[]) => Promise<void>;
}

const WebSocketContext = createContext<WebSocketContextType | undefined>(undefined);

export const WebSocketProvider: FC<{ children: React.ReactNode }> = ({ children }) => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const { isAuthenticated, user } = useAuth();

  useEffect(() => {
    if (!isAuthenticated || !user) {
      return;
    }

    const newConnection = new HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_WEBSOCKET_URL}/hub`, {
        accessTokenFactory: () => localStorage.getItem('accessToken') || '',
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(LogLevel.Information)
      .build();

    // Connection event handlers
    newConnection.onreconnecting(() => {
      console.log('WebSocket reconnecting...');
      setIsConnected(false);
    });

    newConnection.onreconnected(() => {
      console.log('WebSocket reconnected');
      setIsConnected(true);
    });

    newConnection.onclose(() => {
      console.log('WebSocket connection closed');
      setIsConnected(false);
    });

    // Start connection
    const startConnection = async () => {
      try {
        await newConnection.start();
        console.log('WebSocket connected');
        setIsConnected(true);
        
        // Join user-specific groups
        await newConnection.invoke('JoinUserGroup', user.id);
        if (user.role === 'trainer') {
          await newConnection.invoke('JoinTrainerGroup', user.id);
        }
      } catch (error) {
        console.error('WebSocket connection failed:', error);
        setTimeout(startConnection, 5000);
      }
    };

    startConnection();
    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, [isAuthenticated, user]);

  const subscribe = (event: string, handler: (...args: any[]) => void) => {
    if (connection) {
      connection.on(event, handler);
    }
  };

  const unsubscribe = (event: string, handler: (...args: any[]) => void) => {
    if (connection) {
      connection.off(event, handler);
    }
  };

  const send = async (event: string, ...args: any[]) => {
    if (connection && isConnected) {
      await connection.invoke(event, ...args);
    }
  };

  return (
    <WebSocketContext.Provider value={{ connection, isConnected, subscribe, unsubscribe, send }}>
      {children}
    </WebSocketContext.Provider>
  );
};

export const useWebSocket = () => {
  const context = useContext(WebSocketContext);
  if (context === undefined) {
    throw new Error('useWebSocket must be used within a WebSocketProvider');
  }
  return context;
};
```

### 13.2 Real-time Notifications
```typescript
// src/features/notifications/hooks/useNotifications.ts
import { useEffect, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useWebSocket } from '../../../shared/providers/WebSocketProvider';
import { 
  addNotification, 
  markAsRead, 
  removeNotification,
  selectUnreadCount 
} from '../store/notificationSlice';
import { Notification } from '../types';

export const useNotifications = () => {
  const dispatch = useDispatch();
  const { subscribe, unsubscribe } = useWebSocket();
  const unreadCount = useSelector(selectUnreadCount);

  useEffect(() => {
    const handleNewNotification = (notification: Notification) => {
      dispatch(addNotification(notification));
      
      // Show browser notification if permitted
      if (Notification.permission === 'granted') {
        new Notification(notification.title, {
          body: notification.message,
          icon: '/logo192.png',
          badge: '/badge.png',
          tag: notification.id,
        });
      }
    };

    const handleNotificationRead = (notificationId: string) => {
      dispatch(markAsRead(notificationId));
    };

    const handleNotificationDeleted = (notificationId: string) => {
      dispatch(removeNotification(notificationId));
    };

    // Subscribe to WebSocket events
    subscribe('NewNotification', handleNewNotification);
    subscribe('NotificationRead', handleNotificationRead);
    subscribe('NotificationDeleted', handleNotificationDeleted);

    // Request notification permission
    if (Notification.permission === 'default') {
      Notification.requestPermission();
    }

    return () => {
      unsubscribe('NewNotification', handleNewNotification);
      unsubscribe('NotificationRead', handleNotificationRead);
      unsubscribe('NotificationDeleted', handleNotificationDeleted);
    };
  }, [dispatch, subscribe, unsubscribe]);

  return { unreadCount };
};
```

## 14. Performance Optimization

### 14.1 Code Splitting Strategy
```typescript
// src/app/routes/lazyRoutes.ts
import { lazy, Suspense } from 'react';
import { RouteObject } from 'react-router-dom';
import { PageLoader } from '../../shared/components/PageLoader';

// Lazy load with prefetch
const Dashboard = lazy(() => 
  import(/* webpackPrefetch: true */ '../../pages/Dashboard')
);

const WorkoutList = lazy(() => 
  import(/* webpackChunkName: "workouts" */ '../../pages/workouts/WorkoutList')
);

const AthleteProfile = lazy(() => 
  import(/* webpackChunkName: "athletes" */ '../../pages/athletes/AthleteProfile')
);

// Wrapper component for lazy loaded routes
const LazyRoute: FC<{ component: React.LazyExoticComponent<any> }> = ({ component: Component }) => (
  <Suspense fallback={<PageLoader />}>
    <Component />
  </Suspense>
);
```

### 14.2 Memoization & Performance Hooks
```typescript
// src/features/workouts/components/WorkoutList/WorkoutList.tsx
import { memo, useMemo, useCallback, useState, useTransition } from 'react';
import { useVirtual } from 'react-virtual';
import { Workout } from '../../types';

interface WorkoutListProps {
  workouts: Workout[];
  onSelect: (workout: Workout) => void;
  filters: WorkoutFilters;
}

export const WorkoutList = memo<WorkoutListProps>(({ 
  workouts, 
  onSelect, 
  filters 
}) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [isPending, startTransition] = useTransition();

  // Memoize filtered workouts
  const filteredWorkouts = useMemo(() => {
    return workouts.filter(workout => {
      if (searchTerm && !workout.name.toLowerCase().includes(searchTerm.toLowerCase())) {
        return false;
      }
      if (filters.type && workout.type !== filters.type) {
        return false;
      }
      if (filters.dateFrom && new Date(workout.date) < new Date(filters.dateFrom)) {
        return false;
      }
      return true;
    });
  }, [workouts, searchTerm, filters]);

  // Virtualization for large lists
  const parentRef = useRef<HTMLDivElement>(null);
  const rowVirtualizer = useVirtual({
    size: filteredWorkouts.length,
    parentRef,
    estimateSize: useCallback(() => 100, []),
    overscan: 5,
  });

  // Optimized event handler
  const handleSearch = useCallback((value: string) => {
    startTransition(() => {
      setSearchTerm(value);
    });
  }, []);

  // Memoized click handler
  const handleWorkoutClick = useCallback((workout: Workout) => {
    onSelect(workout);
  }, [onSelect]);

  return (
    <Box>
      <SearchBar 
        value={searchTerm} 
        onChange={handleSearch}
        disabled={isPending}
      />
      
      <Box
        ref={parentRef}
        sx={{
          height: '600px',
          overflow: 'auto',
          opacity: isPending ? 0.6 : 1,
        }}
      >
        <Box
          sx={{
            height: `${rowVirtualizer.totalSize}px`,
            width: '100%',
            position: 'relative',
          }}
        >
          {rowVirtualizer.virtualItems.map((virtualRow) => {
            const workout = filteredWorkouts[virtualRow.index];
            return (
              <Box
                key={workout.id}
                sx={{
                  position: 'absolute',
                  top: 0,
                  left: 0,
                  width: '100%',
                  height: `${virtualRow.size}px`,
                  transform: `translateY(${virtualRow.start}px)`,
                }}
              >
                <WorkoutCard
                  workout={workout}
                  onClick={() => handleWorkoutClick(workout)}
                />
              </Box>
            );
          })}
        </Box>
      </Box>
    </Box>
  );
}, (prevProps, nextProps) => {
  // Custom comparison for memo
  return (
    prevProps.workouts === nextProps.workouts &&
    prevProps.filters === nextProps.filters
  );
});

WorkoutList.displayName = 'WorkoutList';
```

### 14.3 Image Optimization
```typescript
// src/shared/components/OptimizedImage/OptimizedImage.tsx
import { useState, useEffect } from 'react';
import { useInView } from 'react-intersection-observer';

interface OptimizedImageProps {
  src: string;
  alt: string;
  width?: number;
  height?: number;
  lazy?: boolean;
  placeholder?: string;
}

export const OptimizedImage: FC<OptimizedImageProps> = ({
  src,
  alt,
  width,
  height,
  lazy = true,
  placeholder = '/placeholder.jpg',
}) => {
  const [imageSrc, setImageSrc] = useState(placeholder);
  const [imageLoaded, setImageLoaded] = useState(false);
  const { ref, inView } = useInView({
    triggerOnce: true,
    threshold: 0.1,
  });

  useEffect(() => {
    if (!lazy || inView) {
      const img = new Image();
      img.src = src;
      img.onload = () => {
        setImageSrc(src);
        setImageLoaded(true);
      };
    }
  }, [src, inView, lazy]);

  return (
    <Box
      ref={ref}
      sx={{
        position: 'relative',
        width,
        height,
        overflow: 'hidden',
      }}
    >
      <img
        src={imageSrc}
        alt={alt}
        style={{
          width: '100%',
          height: '100%',
          objectFit: 'cover',
          filter: imageLoaded ? 'none' : 'blur(10px)',
          transition: 'filter 0.3s ease',
        }}
      />
    </Box>
  );
};
```

### 14.4 Bundle Optimization
```typescript
// vite.config.ts
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { visualizer } from 'rollup-plugin-visualizer';
import { compression } from 'vite-plugin-compression2';

export default defineConfig({
  plugins: [
    react(),
    compression({
      algorithm: 'gzip',
      exclude: [/\.(br)$/, /\.(gz)$/],
    }),
    compression({
      algorithm: 'brotliCompress',
      exclude: [/\.(br)$/, /\.(gz)$/],
    }),
    visualizer({
      open: true,
      gzipSize: true,
      brotliSize: true,
    }),
  ],
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          'react-vendor': ['react', 'react-dom', 'react-router-dom'],
          'redux-vendor': ['@reduxjs/toolkit', 'react-redux', 'redux-persist'],
          'ui-vendor': ['@mui/material', '@mui/icons-material', '@emotion/react'],
          'chart-vendor': ['recharts', 'chart.js', 'react-chartjs-2'],
          'utils': ['lodash', 'date-fns', 'axios'],
        },
      },
    },
    chunkSizeWarningLimit: 1000,
    sourcemap: true,
    minify: 'terser',
    terserOptions: {
      compress: {
        drop_console: true,
        drop_debugger: true,
      },
    },
  },
  optimizeDeps: {
    include: ['react', 'react-dom', '@mui/material'],
  },
});
```

## 15. Testing Strategy

### 15.1 Unit Testing Setup
```typescript
// vitest.config.ts
import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
    coverage: {
      provider: 'c8',
      reporter: ['text', 'json', 'html'],
      exclude: [
        'node_modules/',
        'src/test/',
        '*.config.ts',
        '**/*.d.ts',
        '**/*.stories.tsx',
      ],
    },
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
});

// src/test/setup.ts
import '@testing-library/jest-dom';
import { cleanup } from '@testing-library/react';
import { afterEach, vi } from 'vitest';

afterEach(() => {
  cleanup();
});

// Mock window.matchMedia
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: vi.fn().mockImplementation(query => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: vi.fn(),
    removeListener: vi.fn(),
    addEventListener: vi.fn(),
    removeEventListener: vi.fn(),
    dispatchEvent: vi.fn(),
  })),
});
```

### 15.2 Component Testing
```typescript
// src/features/workouts/components/WorkoutCard/WorkoutCard.test.tsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { vi, describe, it, expect, beforeEach } from 'vitest';
import { WorkoutCard } from './WorkoutCard';
import { mockWorkout } from '../../../../test/mocks/workout';

describe('WorkoutCard', () => {
  const mockProps = {
    workout: mockWorkout,
    onView: vi.fn(),
    onEdit: vi.fn(),
    onDelete: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders workout information correctly', () => {
    render(<WorkoutCard {...mockProps} />);
    
    expect(screen.getByText(mockWorkout.name)).toBeInTheDocument();
    expect(screen.getByText(/January 15, 2024/)).toBeInTheDocument();
    mockWorkout.exercises.forEach(exercise => {
      expect(screen.getByText(exercise.name)).toBeInTheDocument();
    });
  });

  it('calls onView when view button is clicked', async () => {
    const user = userEvent.setup();
    render(<WorkoutCard {...mockProps} />);
    
    const viewButton = screen.getByRole('button', { name: /view/i });
    await user.click(viewButton);
    
    expect(mockProps.onView).toHaveBeenCalledWith(mockWorkout.id);
    expect(mockProps.onView).toHaveBeenCalledTimes(1);
  });

  it('shows confirmation dialog before deletion', async () => {
    const user = userEvent.setup();
    render(<WorkoutCard {...mockProps} />);
    
    const deleteButton = screen.getByRole('button', { name: /delete/i });
    await user.click(deleteButton);
    
    expect(screen.getByText(/Are you sure you want to delete this workout?/)).toBeInTheDocument();
    
    const confirmButton = screen.getByRole('button', { name: /confirm/i });
    await user.click(confirmButton);
    
    await waitFor(() => {
      expect(mockProps.onDelete).toHaveBeenCalledWith(mockWorkout.id);
    });
  });

  it('applies correct styling based on workout type', () => {
    const strengthWorkout = { ...mockWorkout, type: 'strength' };
    const { rerender } = render(<WorkoutCard {...mockProps} workout={strengthWorkout} />);
    
    expect(screen.getByTestId('workout-card')).toHaveClass('workout-card--strength');
    
    const cardioWorkout = { ...mockWorkout, type: 'cardio' };
    rerender(<WorkoutCard {...mockProps} workout={cardioWorkout} />);
    
    expect(screen.getByTestId('workout-card')).toHaveClass('workout-card--cardio');
  });
});
```

### 15.3 Integration Testing
```typescript
// src/features/workouts/WorkoutFlow.integration.test.tsx
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { rest } from 'msw';
import { setupServer } from 'msw/node';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { configureStore } from '@reduxjs/toolkit';
import { WorkoutPage } from '../../pages/workouts/WorkoutPage';
import workoutReducer from './store/workoutSlice';

const server = setupServer(
  rest.get('/api/workouts', (req, res, ctx) => {
    return res(
      ctx.json({
        items: [
          { id: '1', name: 'Morning Workout', type: 'strength' },
          { id: '2', name: 'Evening Cardio', type: 'cardio' },
        ],
        total: 2,
      })
    );
  }),
  rest.post('/api/workouts', (req, res, ctx) => {
    return res(
      ctx.json({
        id: '3',
        name: req.body.name,
        type: req.body.type,
      })
    );
  })
);

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());

describe('Workout Flow Integration', () => {
  const renderWithProviders = (component: React.ReactElement) => {
    const store = configureStore({
      reducer: {
        workouts: workoutReducer,
      },
    });

    return render(
      <Provider store={store}>
        <BrowserRouter>
          {component}
        </BrowserRouter>
      </Provider>
    );
  };

  it('completes full workout creation flow', async () => {
    const user = userEvent.setup();
    renderWithProviders(<WorkoutPage />);

    // Wait for initial data load
    await waitFor(() => {
      expect(screen.getByText('Morning Workout')).toBeInTheDocument();
    });

    // Click create button
    const createButton = screen.getByRole('button', { name: /create workout/i });
    await user.click(createButton);

    // Fill form
    const nameInput = screen.getByLabelText(/workout name/i);
    await user.type(nameInput, 'New Test Workout');

    const typeSelect = screen.getByLabelText(/workout type/i);
    await user.selectOptions(typeSelect, 'strength');

    // Submit form
    const submitButton = screen.getByRole('button', { name: /save/i });
    await user.click(submitButton);

    // Verify new workout appears
    await waitFor(() => {
      expect(screen.getByText('New Test Workout')).toBeInTheDocument();
    });
  });
});
```

### 15.4 E2E Testing
```typescript
// cypress/e2e/workout-management.cy.ts
describe('Workout Management E2E', () => {
  beforeEach(() => {
    cy.login('trainer@example.com', 'password123');
    cy.visit('/workouts');
  });

  it('creates, edits, and deletes a workout', () => {
    // Create workout
    cy.get('[data-testid="create-workout-btn"]').click();
    cy.get('input[name="name"]').type('E2E Test Workout');
    cy.get('textarea[name="description"]').type('This is a test workout');
    cy.get('select[name="type"]').select('strength');
    
    // Add exercises
    cy.get('[data-testid="add-exercise-btn"]').click();
    cy.get('[data-testid="exercise-search"]').type('Bench Press');
    cy.get('[data-testid="exercise-option-bench-press"]').click();
    cy.get('input[name="exercises.0.sets"]').clear().type('4');
    cy.get('input[name="exercises.0.reps"]').clear().type('12');
    
    cy.get('[data-testid="save-workout-btn"]').click();
    
    // Verify creation
    cy.contains('E2E Test Workout').should('be.visible');
    cy.contains('Workout created successfully').should('be.visible');
    
    // Edit workout
    cy.get('[data-testid="workout-card-e2e-test-workout"]')
      .find('[data-testid="edit-btn"]')
      .click();
    
    cy.get('input[name="name"]').clear().type('Updated E2E Workout');
    cy.get('[data-testid="save-workout-btn"]').click();
    
    // Verify edit
    cy.contains('Updated E2E Workout').should('be.visible');
    
    // Delete workout
    cy.get('[data-testid="workout-card-updated-e2e-workout"]')
      .find('[data-testid="delete-btn"]')
      .click();
    
    cy.get('[data-testid="confirm-delete-btn"]').click();
    
    // Verify deletion
    cy.contains('Updated E2E Workout').should('not.exist');
    cy.contains('Workout deleted successfully').should('be.visible');
  });

  it('logs a workout session', () => {
    cy.get('[data-testid="workout-card-morning-routine"]').click();
    cy.get('[data-testid="start-workout-btn"]').click();
    
    // Log first exercise
    cy.get('[data-testid="exercise-0"]').within(() => {
      cy.get('input[name="weight"]').type('60');
      cy.get('input[name="reps"]').type('10');
      cy.get('[data-testid="complete-set-btn"]').click();
    });
    
    // Use rest timer
    cy.get('[data-testid="rest-timer"]').should('be.visible');
    cy.wait(3000);
    
    // Complete workout
    cy.get('[data-testid="finish-workout-btn"]').click();
    cy.get('[data-testid="workout-notes"]').type('Great session!');
    cy.get('[data-testid="save-session-btn"]').click();
    
    // Verify completion
    cy.contains('Workout completed').should('be.visible');
    cy.url().should('include', '/workouts/history');
  });
});
```

## 16. Deployment & CI/CD

### 16.1 Docker Configuration
```dockerfile
# Dockerfile
FROM node:18-alpine AS builder

WORKDIR /app

# Copy package files
COPY package*.json ./
RUN npm ci --only=production

# Copy source code
COPY . .

# Build application
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=builder /app/dist /usr/share/nginx/html

# Add health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost || exit 1

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

### 16.2 GitHub Actions CI/CD
```yaml
# .github/workflows/deploy.yml
name: Deploy Frontend

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

env:
  NODE_VERSION: '18'
  
jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Run linting
        run: npm run lint
      
      - name: Run type checking
        run: npm run type-check
      
      - name: Run unit tests
        run: npm run test:unit -- --coverage
      
      - name: Upload coverage
        uses: codecov/codecov-action@v3
        with:
          file: ./coverage/coverage-final.json
  
  build:
    needs: test
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Build application
        run: npm run build
        env:
          VITE_API_BASE_URL: ${{ secrets.API_BASE_URL }}
          VITE_WEBSOCKET_URL: ${{ secrets.WEBSOCKET_URL }}
      
      - name: Upload build artifacts
        uses: actions/upload-artifact@v3
        with:
          name: dist
          path: dist/
  
  deploy:
    needs: build
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Download build artifacts
        uses: actions/download-artifact@v3
        with:
          name: dist
          path: dist/
      
      - name: Deploy to Azure Static Web Apps
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: 'upload'
          app_location: 'dist'
          skip_app_build: true
```

### 16.3 Environment Configuration
```typescript
// src/config/environment.ts
interface Environment {
  apiBaseUrl: string;
  websocketUrl: string;
  storageUrl: string;
  stripePublicKey: string;
  googleMapsKey: string;
  sentryDsn: string;
  gaTrackingId: string;
  environment: 'development' | 'staging' | 'production';
  features: {
    enableChat: boolean;
    enablePayments: boolean;
    enableAnalytics: boolean;
    enablePWA: boolean;
  };
}

const config: Record<string, Environment> = {
  development: {
    apiBaseUrl: 'http://localhost:5000/api',
    websocketUrl: 'ws://localhost:5000/hub',
    storageUrl: 'http://localhost:5000/storage',
    stripePublicKey: 'pk_test_...',
    googleMapsKey: '',
    sentryDsn: '',
    gaTrackingId: '',
    environment: 'development',
    features: {
      enableChat: true,
      enablePayments: false,
      enableAnalytics: false,
      enablePWA: false,
    },
  },
  staging: {
    apiBaseUrl: 'https://api-staging.example.com/api',
    websocketUrl: 'wss://api-staging.example.com/hub',
    storageUrl: 'https://storage-staging.example.com',
    stripePublicKey: 'pk_test_...',
    googleMapsKey: 'AIza...',
    sentryDsn: 'https://...@sentry.io/...',
    gaTrackingId: 'G-...',
    environment: 'staging',
    features: {
      enableChat: true,
      enablePayments: true,
      enableAnalytics: true,
      enablePWA: true,
    },
  },
  production: {
    apiBaseUrl: 'https://api.example.com/api',
    websocketUrl: 'wss://api.example.com/hub',
    storageUrl: 'https://storage.example.com',
    stripePublicKey: 'pk_live_...',
    googleMapsKey: 'AIza...',
    sentryDsn: 'https://...@sentry.io/...',
    gaTrackingId: 'G-...',
    environment: 'production',
    features: {
      enableChat: true,
      enablePayments: true,
      enableAnalytics: true,
      enablePWA: true,
    },
  },
};

export const environment = config[import.meta.env.VITE_APP_ENV || 'development'];
```

---

## Appendix A: Code Style Guide

### TypeScript Conventions
- Use explicit types for function parameters and return values
- Prefer interfaces over types for object shapes
- Use enums for fixed sets of values
- Enable strict mode in tsconfig.json

### React Best Practices
- Use functional components with hooks
- Implement proper error boundaries
- Use React.memo for expensive components
- Implement proper loading and error states
- Use Suspense for code splitting

### State Management Guidelines
- Keep Redux store normalized
- Use RTK Query for server state
- Implement proper selector patterns
- Use Redux DevTools in development

### Testing Requirements
- Minimum 80% code coverage
- Test user interactions, not implementation
- Use MSW for API mocking
- Write E2E tests for critical flows

---

## Appendix B: Performance Metrics

### Target Metrics
- First Contentful Paint: < 1.5s
- Time to Interactive: < 3.5s
- Cumulative Layout Shift: < 0.1
- Bundle size: < 300KB (gzipped)
- API response time: < 200ms
- WebSocket latency: < 100ms

### Monitoring Tools
- Lighthouse CI
- Sentry Performance Monitoring
- Google Analytics
- Custom performance marks
- Real User Monitoring (RUM)