# Online Courses Platform — Technical Assessment

[![.NET](https://img.shields.io/badge/.NET-8-blue)](https://dotnet.microsoft.com/)
[![Status](https://img.shields.io/badge/status-Completed-success)]()

## Overview

This project is a **technical assessment** to evaluate backend and frontend skills. The goal is to build a **REST API with .NET** for managing courses and lessons, along with a **decoupled frontend** consuming the API.  

The solution demonstrates **Clean Architecture principles**, proper **business rule implementation**, and separation of concerns.

---

## Features

### Backend
- .NET 8+
- Entity Framework Core
- Relational Database (PostgreSQL or MySQL)
- EF Core Migrations
- Authentication with **Identity + JWT**
- REST API for courses and lessons
- Soft deletion and global `IsDeleted` filter

### Frontend
- HTML, CSS, JavaScript or modern framework (React, Vue, Angular)
- JWT-based authenticated calls
- Dashboard with statistics
- Course and Lesson management (CRUD + reordering)
- Filter and pagination

---

## Data Model

**Course**
- `Id` (GUID)
- `Title` (string)
- `Status` (Draft | Published)
- `IsDeleted` (bool)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

**Lesson**
- `Id` (GUID)
- `CourseId` (GUID)
- `Title` (string)
- `Order` (int)
- `IsDeleted` (bool)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime)

**Relationships**
- A course can have many lessons
- A lesson belongs to one course

---

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|---------|------|-------------|
| POST   | `/api/auth/register` | No | Register new user |
| POST   | `/api/auth/login` | No | Login and receive JWT |
| PATCH  | `/api/courses/{id}/publish` | Yes | Publish course (if rules met) |
| PATCH  | `/api/courses/{id}/unpublish` | Yes | Unpublish course |
| GET    | `/api/courses/search?q=&status=&page=&pageSize=` | Yes | List/filter courses |
| GET    | `/api/courses/{id}/summary` | Yes | Course summary with total lessons and last update |

---

## Business Rules
- Courses can **only be published if at least one active lesson exists**
- Lesson `Order` must be **unique per course**
- Deletion is **soft delete**, not physical
- Reordering lessons **must not produce duplicate orders**
- `/publish` only publishes courses satisfying the rules
- `/summary` returns:
  - Basic course info
  - Total lessons
  - Last modified date

> All rules are enforced in the backend, not the frontend.

---

## Frontend Features

**Courses**
- List with pagination and status filter (Draft / Published)
- Create, edit, delete (soft delete)
- Publish/unpublish

**Lessons**
- List by course (ordered by `Order`)
- Create, edit, delete (soft delete)
- Reorder lessons (move up/down)

**Authentication**
- Simple login screen
- JWT token storage and use for API calls

---

## Setup Instructions

### Backend
1. Clone the repository  
   ```bash
   git clone https://github.com/yourusername/online-courses-platform.git
