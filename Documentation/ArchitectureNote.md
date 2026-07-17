# System Architecture

## Overview

The Classic Barber Salon Management System is built using a layered architecture with **ASP.NET Core 8 Razor Pages**, **Entity Framework Core**, and **PostgreSQL**.

The purpose of this architecture is to separate responsibilities between the user interface, business logic, and database access. This makes the system easier to maintain, test, and extend.

The project structure is organized as follows:

```
BarberSalon
│
├── Pages
│   └── Razor Pages interfaces for Customer, Employee, and Administrator
│
├── Models
│   └── Entity classes representing database tables
│
├── Data
│   └── AppDbContext and Entity Framework Core configuration
│
├── Services
│   └── Application business logic and operations
│
├── Areas
│   └── Identity pages and authentication management
│
├── Migrations
│   └── Database schema changes managed by EF Core
│
└── wwwroot
    └── Static files (CSS, JavaScript, images)
```

---

# Architecture Components

## 1. Presentation Layer (Razor Pages)

The presentation layer is responsible for handling user requests and displaying information.

The system provides different interfaces depending on the user role.

## Customer Interface

Customers can:

- Register and login.
- Browse available services and products.
- Book appointments with available employees.
- Manage their appointments.
- Add products to the shopping cart.
- Place orders.
- Manage their profile.

## Employee Interface

Employees can:

- Access their dashboard.
- View assigned appointments.
- Check their working schedule.
- Update appointment status.

## Administrator Interface

Administrators can:

- Access a comprehensive dashboard containing store statistics, revenue analytics, and interactive charts.
- Manage products, categories, and services.
- Manage employees.
- Assign services to employees.
- Configure working hours.
- Manage appointments and orders.

---

# 2. Business Logic Layer (Services)

The service layer contains the main application logic.

Instead of placing complex operations directly inside Razor Pages, the logic is handled through dedicated services.

Responsibilities include:

- Checking employee availability.
- Creating and managing appointments.
- Managing products and services.
- Handling image uploads through Supabase Storage.
- Managing customer orders.

The communication flow is:

```
Razor Page
     |
     v
Service Layer
     |
     v
AppDbContext
     |
     v
PostgreSQL Database
```

This separation keeps the code cleaner and easier to modify.

---

# 3. Data Access Layer

The application uses Entity Framework Core as the ORM with PostgreSQL as the database system.

The main database context is:

```
AppDbContext : IdentityDbContext<ApplicationUser>
```

It manages access to:

- Users
- Employees
- Services
- Products
- Categories
- Appointments
- Orders
- Payments
- Working Hours


Entity Framework Core is responsible for:

- Mapping C# classes to database tables.
- Managing relationships between entities.
- Executing LINQ queries.
- Applying database migrations.

---

# Database Relationships

## ApplicationUser - Employee

Relationship:

```
ApplicationUser 1 -------- 0..1 Employee
```

A user account can optionally have an employee profile.

This separates authentication information from employee business data.

Example:

```
ApplicationUser
       |
       | UserId
       |
    Employee
```

Configuration:

```csharp
HasOne(e => e.User)
.WithOne(u => u.Employee)
```

---

## Customer - Appointment

Relationship:

```
ApplicationUser 1 -------- * Appointment
```

A customer can create multiple appointments.

If a customer account is deleted, related appointments are automatically deleted:

```csharp
.OnDelete(DeleteBehavior.Cascade)
```

---

## Employee - Appointment

Relationship:

```
Employee 1 -------- * Appointment
```

One employee can have multiple appointments.

Each appointment belongs to one employee.

---

## Service - Appointment

Relationship:

```
Service 1 -------- * Appointment
```

A service can be booked many times by different customers.

Example:

```
Hair Cut
   |
   |-- Appointment 1
   |-- Appointment 2
   |-- Appointment 3
```

---

## Employee - Service

Relationship:

```
Employee * -------- * Service
```

This many-to-many relationship is implemented using:

```
EmployeeService
```

Example:

| Employee | Service |
|----------|---------|
|   John   | Hair Cut|
|   John   |Beard Trim|
|   Mike   | Hair Cut|

A unique constraint prevents assigning the same service twice to the same employee:

```csharp
HasIndex(es => new
{
    es.EmployeeId,
    es.ServiceId
})
.IsUnique();
```

---

## Category - Product

Relationship:

```
Category 1 -------- * Product
```

A category can contain multiple products.

Example:

```
Hair Products
      |
      |-- Shampoo
      |-- Gel
      |-- Wax
```

---

## Order - Payment

Relationship:

```
Order 1 -------- 1 Payment
```

Each order has one payment record.

Configuration:

```csharp
HasOne(p => p.Order)
.WithOne(o => o.Payment)
```

---

# Authentication and Authorization

The application uses **ASP.NET Core Identity** for authentication and security management.

## Authentication

Identity handles:

- User registration and login.
- Password hashing.
- Authentication cookies.
- User account management.


The user entity extends Identity:

```
IdentityUser
        |
        |
ApplicationUser
    
```

---

## Authorization

The system uses role-based authorization.

Available roles:

```
Customer
Employee
Administrator
```

Each role can access only the pages related to its responsibilities.

Example:

```csharp
[Authorize(Roles="Admin")]
```

Only administrators can access administration pages.

---

# Appointment Conflict Prevention

Preventing double booking is an important part of the appointment system.

The application uses multiple validation levels.

---

## 1. Application-Level Validation

Before creating a new appointment, the system checks whether the employee already has an appointment at the selected time.

The check is based on:

```
EmployeeId
+
AppointmentDate
+
StartTime
```

Example:

```
Employee: John

10/08/2026 - 09:30
Existing appointment


New booking:
10/08/2026 - 09:30

Result:
Rejected
```

---

## 2. Database-Level Protection

To prevent race conditions, the database also enforces uniqueness.

The appointment table contains a unique index:

```csharp
builder.Entity<Appointment>()
.HasIndex(a => new
{
    a.EmployeeId,
    a.AppointmentDate,
    a.StartTime
})
.IsUnique();
```

This guarantees that the same employee cannot have two appointments at the same date and time.

Even if two users submit a booking at the same moment, PostgreSQL will prevent duplicate records.

---

## 3. Working Hours Validation

Appointments must be within the employee working schedule.

The system checks:

```
Appointment Time
        |
        must be inside
        |
Employee Working Hours
```

Example:

Employee schedule:

```
09:00 - 17:00
```

Allowed:

```
10:30
```

Rejected:

```
18:00
```

---

## 4. Employee Service Validation

A customer can only select employees who provide the requested service.

The system checks the:

```
EmployeeService
```

relationship before creating an appointment.

Example:

```
Service:
Hair Coloring

Available:
John
Mike

Unavailable ( here mean not appear ):
David
```

---

# Database Optimization

Indexes are added to improve frequently used queries.

## Employee Service

Index added on:

```
(EmployeeId, ServiceId)
```

---

## Appointment Availability Search

Improves searching for employee availability:

```
(EmployeeId, AppointmentDate, StartTime)
```

---

## Employee Search

Index added on:

```
Employee.Name
```

---

## Service Search

Index added on:

```
Service.Name
```

---

# Data Integrity

The database uses:

- Foreign keys to maintain relationships.
- Unique constraints to prevent duplicate data.
- Delete behaviors for controlling related records.
- Decimal precision for money values.
- Time-only columns for schedules.

Examples:

Money fields:

```csharp
HasPrecision(18,2)
```

Time fields:

```csharp
HasColumnType("time")
```