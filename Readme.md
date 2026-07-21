﻿# Classic Barber Salon Management System

## Project Overview

Classic Barber Salon Management System is a full-stack web application developed using **ASP.NET Core 8 Razor Pages** to simplify the daily operations of a barber salon.

The application provides three different user roles: **Customer**, **Employee**, and **Administrator**, each with a dedicated interface and permissions based on their responsibilities.

Customers can browse services and products, book appointments with available barbers, manage their appointments, purchase salon products through an integrated shopping cart, and track their payment status.

Employees have access to their personal dashboard where they can view their daily schedule, manage assigned appointments, and update appointment statuses.

Administrators can manage every aspect of the salon including employees, services, categories, products, appointments, working schedules, orders, and customer accounts.

The project follows a layered architecture using Razor Pages, Entity Framework Core, ASP.NET Core Identity, and PostgreSQL while applying dependency injection, role-based authorization, and service abstraction to keep the code maintainable and scalable.



---

# Technologies Used

* **ASP.NET Core 8 Razor Pages** – Web application framework
* **Entity Framework Core (EF Core)** – Object-Relational Mapping (ORM)
* **PostgreSQL 16** – Relational database management system
* **ASP.NET Core Identity** – Authentication and role-based authorization
* **Bootstrap 5** – Responsive user interface design
* **Bootstrap Icons** – User interface icons
* **LINQ** – Data querying and manipulation
* **Dependency Injection (DI)** – Service registration and dependency management
* **Service Layer Architecture** – Separation of business logic from presentation layer
* **Supabase Storage** – Image storage for products, services, and customer profiles
* **Docker & Docker Compose** – Containerized deployment
* **Git & GitHub** – Version control and source code management

---

# Local Setup

## Prerequisites

Before running the project locally, make sure you have the following installed:

* .NET 8 SDK
* PostgreSQL
* Visual Studio 2022 (or Visual Studio Code)
* Git

---

## Clone the Repository

```bash
git clone https://github.com/Hasan123-cs/Barber-Salon-Booking-Platform.git

```
cd Barber-Salon-Booking-Platform

---

## Restore Dependencies

```bash
dotnet restore
```

---

# Environment Variables

Create a `.env` file in the project root.

Example:

```env
CONNECTION_STRING=Host=localhost;Port=5432;Database=BarberSalonDB;Username=postgres;Password=YOUR_PASSWORD

SUPABASE_URL=Your Supabase URL

SUPABASE_KEY=Your Supabase API Key
```

> **Note:** Never commit your actual `.env` file or any real credentials to the repository.

---

# Database Initialization

The application automatically applies pending Entity Framework Core migrations on startup using:

```csharp
db.Database.Migrate();
```

No manual migration command is required.

---

# How to Run the Project

Run the application using the .NET CLI:

```bash
dotnet run
```

Or open the solution in Visual Studio and press **F5**.

The application will launch in your default web browser.

---

# Demo Users

If database seeding is enabled, use the following accounts:

## Administrator

```
Email:
admin@example.com

Password:
Admin123
```

## Employee

```
Email:
employee@example.com

Password:
Employee123
```

## Customer

```
Email:
customer@example.com

Password:
Customer123
```

---

# Deployment Steps (Docker)

The project supports deployment using **Docker Compose**.

The Docker environment contains two services:

* **web**: ASP.NET Core 8 Razor Pages application.
* **postgres**: PostgreSQL 16 database server.

Architecture:

```
Browser

    |

localhost:8080

    |

barber-web

ASP.NET Core Application

    |

Docker Network

    |

barber-db

PostgreSQL Database
```

---

# Docker Requirements

Install:

* Docker Desktop

Verify installation:

```bash
docker --version

docker compose version
```

---

# Docker Environment Variables

Create a `.env` file in the same directory as:

```
docker-compose.yml
```

Add:

```env
DB_PASSWORD=YOUR_DATABASE_PASSWORD
```

This password is used by:

* PostgreSQL container.
* ASP.NET Core application database connection.

---

# Build and Start Services

Build the Docker image and start all containers:

```bash
docker compose up --build
```

Run containers in background:

```bash
docker compose up -d
```

This command will:

* Create the PostgreSQL container.
* Build the ASP.NET Core Docker image.
* Create the application container.
* Connect both containers through Docker networking.

---

# Database Connection in Docker

Inside Docker, the application connects to PostgreSQL using:

```
Host=postgres
```

not:

```
Host=localhost
```

because Docker Compose allows containers to communicate using service names.

The connection string is automatically created:

```
Host=postgres;
Port=5432;
Database=BarberSalonDB;
Username=postgres;
Password=DB_PASSWORD
```

---

# Database Migration in Docker

Database migrations are applied automatically when the application starts.

The application executes:

```csharp
db.Database.Migrate();
```

Startup flow:

```
PostgreSQL container starts

        ↓

Healthcheck verifies database availability

        ↓

ASP.NET Core container starts

        ↓

EF Core applies migrations

        ↓

Application becomes available
```

No manual migration command is required.

---

# Access the Application

After containers start, open:

```
http://localhost:8080
```

---

# Verify Deployment

Check running containers:

```bash
docker ps
```

Expected containers:

```
barber-web
barber-db
```

---

# View Logs

Application logs:

```bash
docker logs barber-web
```

Database logs:

```bash
docker logs barber-db
```

---

# Stop Services

Stop containers:

```bash
docker compose down
```

Remove containers and database volume:

```bash
docker compose down -v
```

> Warning: Removing the volume deletes PostgreSQL database data.

---

# Rebuild After Code Changes

After modifying the application:

```bash
docker compose up --build
```

Docker will rebuild the ASP.NET Core image and restart the containers.

# Deployment Verification

After deployment:

- Open the application in the browser.
- Test login functionality.
- Create a customer appointment.
- Create an order.
- Verify employee workflow.
- Verify administrator management features.
