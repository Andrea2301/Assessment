# Online Courses Platform — Technical Assessment

[![.NET](https://img.shields.io/badge/.NET-9-blue)](https://dotnet.microsoft.com/)
[![Status](https://img.shields.io/badge/status-Completed-success)]()

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Frontend Features](#frontend-features)
- [Technologies Used](#technologies-used)



## Overview

This project is a technical assessment designed to evaluate full-stack development skills. It consists of a REST API built with .NET 8 for managing online courses and lessons, paired with a decoupled frontend that consumes the API.

The solution demonstrates Clean Architecture principles, proper business rule implementation, and separation of concerns between layers.

## Features

### Backend Capabilities

- Built with .NET 8+ and Entity Framework Core
- Relational database support (MySQL)
- Code-first approach with EF Core Migrations
- Authentication and authorization using ASP.NET Core Identity and JWT
- RESTful API design for courses and lessons management
- Soft deletion pattern with global query filters
- Comprehensive business rule validation

### Frontend Capabilities

- Modern web interface using HTML, CSS, and JavaScript
- JWT-based authentication for secure API calls
- Dashboard with statistical insights
- Full CRUD operations for courses and lessons
- Lesson reordering functionality
- Advanced filtering and pagination

## Architecture

The application follows Clean Architecture principles with clear separation of concerns:

- **Presentation Layer**: MVC frontend and API controllers
- **Application Layer**: Business logic and use cases
- **Domain Layer**: Core entities and business rules
- **Infrastructure Layer**: Data access and external services


### Relationships

- A course can have multiple lessons (one-to-many)
- A lesson belongs to exactly one course (many-to-one)
- Cascading rules apply for related entities

## Prerequisites

Before running this project, ensure you have the following installed:

- .NET 9 SDK or later
- MySQL database server
- A code editor (Visual Studio, VS Code, or Rider recommended)
- Git for version control

## Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/online-courses-platform.git
cd online-courses-platform
```

2. Restore NuGet packages:

```bash
dotnet restore
```

3. Navigate to the Infrastructure project directory to create migrations:

```bash
cd Assessment/OnlineCourses.Infrastructure
```

4. Create the initial migration:

```bash
dotnet ef migrations add Last `
  --startup-project ../OnlineCourses.Api

```

5. Apply database migrations:

```bash
dotnet ef database update `
  --startup-project ../OnlineCourses.Api

```

6. Navigate to the API project directory:

```bash
cd Assessment/OnlineCourses.Api
```

7. Apply database migrations:

```bash
dotnet ef database update
```

## Configuration

Update the `appsettings.json` file in the API project with your database credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=your-server;port=your-port;database=CoursesDB;user=your-user;password=your-password;SslMode=Required;"
  },
  "Jwt": {
    "Key": "your-secure-key-minimum-32-characters-long",
    "Issuer": "OnlineCourses.Api",
    "Audience": "OnlineCourses.Client"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Security Note**: Never commit sensitive credentials to version control. Use user secrets or environment variables in production.

## Running the Application

### Starting the Backend API

1. Navigate to the API project:

```bash
cd Assessment/OnlineCourses.Api
```

2. Run the application:

```bash
dotnet run
```

The API will be available at `http://localhost:5238` (or as configured in `launchSettings.json`).

### Starting the Frontend

1. Open a new terminal and navigate to the MVC project:

```bash
cd Assessment/OnlineCourses.Mvc

```

2. Run the application:

```bash
dotnet run
```

The frontend will be available at `http://localhost:5105` (or as configured in `launchSettings.json`).

## API Documentation

### Authentication Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| POST | `/api/auth/register` | No | Register a new user account |
| POST | `/api/auth/login` | No | Login and receive JWT token |

### Course Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| GET | `/api/courses` | Yes | List all courses |
| GET | `/api/courses/{id}` | Yes | Get course by ID |
| POST | `/api/courses` | Yes | Create new course |
| PUT | `/api/courses/{id}` | Yes | Update course |
| DELETE | `/api/courses/{id}` | Yes | Soft delete course |
| PATCH | `/api/courses/{id}/publish` | Yes | Publish course (if rules met) |
| PATCH | `/api/courses/{id}/unpublish` | Yes | Unpublish course |
| GET | `/api/courses/search` | Yes | Search and filter courses |
| GET | `/api/courses/{id}/summary` | Yes | Get course summary with statistics |

### Lesson Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| GET | `/api/lessons` | Yes | List all lessons |
| GET | `/api/lessons/{id}` | Yes | Get lesson by ID |
| POST | `/api/lessons` | Yes | Create new lesson |
| PUT | `/api/lessons/{id}` | Yes | Update lesson |
| DELETE | `/api/lessons/{id}` | Yes | Soft delete lesson |
| PATCH | `/api/lessons/{id}/reorder` | Yes | Change lesson order |

### Query Parameters for Search

- `q`: Search term (filters by title)
- `status`: Filter by course status (Draft/Published)
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10)




### Authentication

- User-friendly login interface
- Secure JWT token storage (sessionStorage/localStorage)
- Automatic token injection in API requests
- Session management and logout functionality



## Technologies Used

### Backend

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- ASP.NET Core Identity
- JWT Bearer Authentication
- PostgreSQL/MySQL

### Frontend

- HTML5
- CSS3
- JavaScript 
- Fetch API for HTTP requests
- Bootstrap & Bulma

### Development Tools

- Entity Framework Core CLI
- .NET CLI
- Git

