# Full Stack Notes Management System

A production-ready Full Stack Notes Management System built with Vue 3, TypeScript, Tailwind CSS, Pinia, and ASP.NET Core 8 Web API.

## 🎬 Live Demo

<p align="center">
  <img src="docs/demo.webp" alt="Notes App Live Demo" width="800" />
</p>

## 🚀 Features

- **Authentication & Security**
  - JWT Access Tokens & Refresh Tokens
  - Token Rotation and Revocation
  - Secure Password Hashing (BCrypt)
  - Role-based Authorization (User/Admin)
- **Notes Management**
  - Create, Read, Update, Delete (CRUD) operations
  - User-owned data isolation
- **Advanced Data Handling**
  - Server-side Pagination
  - Case-insensitive Search (Title & Content)
  - Sorting (CreatedAt, UpdatedAt, Title / Asc & Desc)
  - Date Filtering (Today, Last 7 Days, Last 30 Days, All)
- **Modern UI/UX**
  - Vue 3 Composition API & Pinia state management
  - Responsive Mobile-First Design with Tailwind CSS
  - Dark/Light Mode with localStorage persistence
  - Smooth Transitions & Animations
  - Skeleton Loading & Empty States
  - Toast Notifications & Modals
- **Backend Architecture**
  - Clean Architecture (Core, Infrastructure, Application, API layers)
  - SOLID Principles & Repository Pattern
  - Dapper Micro-ORM for high performance
  - FluentValidation for request validation
  - Global Exception Handling Middleware
  - Serilog Logging (Console & File)
  - Swagger/OpenAPI Documentation

---

## 📁 Project Architecture

```
notes-app/
├── backend/
│   └── NotesApp/
│       ├── NotesApp.Core/             # Domain Entities, Interfaces, DTOs
│       ├── NotesApp.Infrastructure/   # Dapper Repositories, DB Context, Services
│       ├── NotesApp.Application/      # Business Logic, Validators, Services
│       └── NotesApp.Api/              # Controllers, Middleware, API Config
├── frontend/                          # Vue 3 + Vite + TypeScript + Tailwind
├── database/                          # SQL Scripts (Create DB, Tables, Seed Data)
└── docs/                              # API Postman Collection
```

---

## 🛠 Prerequisites

### For Docker Setup (Recommended)

- [Docker](https://www.docker.com/products/docker-desktop) and [Docker Compose](https://docs.docker.com/compose/install/)

### For Manual Setup

- [Node.js](https://nodejs.org/) (v18+)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express/Developer)
- [Git](https://git-scm.com/)

---

## 💻 Installation & Setup

### Method 1: Using Docker Compose (Recommended & Easiest)

1. Make sure Docker Desktop is running.
2. Open a terminal in the root folder of the project (`notes-app`).
3. Run the following command:
   ```bash
   docker compose up -d --build
   ```
4. Wait a few moments for the database to initialize.
5. The application is now running at:
   - **Frontend:** [http://localhost:5173](http://localhost:5173)
   - **Backend API:** [http://localhost:8080/swagger](http://localhost:8080/swagger)
6. _(Optional)_ If you wish to seed data, you can execute the `database/003_SeedData.sql` script on the database container. This script creates an Admin (`admin@notesapp.com`) and User (`john@notesapp.com`) with the password `Password@123`.

---

### Method 2: Manual Setup

#### 1. Database Setup

1. Connect to your SQL Server instance via SSMS or Azure Data Studio.
2. Execute the scripts in the `database` folder in order:
   - `001_CreateDatabase.sql`
   - `002_CreateTables.sql`
   - `003_SeedData.sql` (Optional)

_Note: The optional seed script creates an Admin (`admin@notesapp.com`) and User (`john@notesapp.com`). Both use the password: `Password@123`_

#### 2. Backend Setup

1. Navigate to the API folder:
   ```bash
   cd backend/NotesApp/NotesApp.Api
   ```
2. Update the `DefaultConnection` string in `appsettings.json` if necessary (defaults to localhost).
3. Run the API:
   ```bash
   dotnet run
   ```
4. The API will start on `http://localhost:5000` (or `https://localhost:5001`).
5. Open `http://localhost:5000/swagger` to view the API documentation.

#### 3. Frontend Setup

1. Navigate to the frontend folder:
   ```bash
   cd frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   npm run dev
   ```
4. Open the app at `http://localhost:5173`.

---

## 📖 API Documentation (Swagger)

Swagger UI is available out-of-the-box.

1. Run the backend API.
2. Navigate to `https://localhost:5001/swagger`.
3. You can test endpoints directly. For authenticated endpoints, use the `/api/auth/login` endpoint to get an `accessToken`, click the "Authorize" button at the top of Swagger, and paste the token as: `Bearer <your_token>`.

---

## 📝 Git Strategy (Conventional Commits)

This repository follows conventional commits:

- `feat`: A new feature (e.g., `feat(auth): implement JWT login`)
- `fix`: A bug fix (e.g., `fix(notes): resolve pagination bug`)
- `docs`: Documentation changes
- `style`: Formatting, missing semi colons, etc
- `refactor`: Refactoring production code
- `test`: Adding tests

---

## 🚢 Deployment Guide

- **Backend**: Publish via `dotnet publish -c Release`. Host on Azure App Service, AWS EC2, or Docker. Ensure ASPNETCORE_ENVIRONMENT is set to "Production".
- **Frontend**: Build via `npm run build`. Host the `dist` folder on Vercel, Netlify, or Azure Static Web Apps.
- **Database**: Deploy SQL Server on Azure SQL, AWS RDS, or a managed VM. Run the creation scripts. Ensure firewalls allow backend connection.

---

Built with using ASP.NET Core & Vue.

---

## 📝 Author

Zin Min
