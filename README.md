# TicketDesk Training Curriculum 

Complete training curriculum, technical objectives, and weekly deliverables for the TicketDesk full-stack application.

---

## 📅 Week 1: C# + OOP + Git
- **Objective:** Master C# fundamentals, core object-oriented principles, and git collaboration workflows[cite: 1].
- **Key Concepts:**
  - Value types vs reference types, control flow (`switch`, `if`), loops, and methods[cite: 1].
  - OOP pillars: Encapsulation (`public`/`private`), Classes vs Objects, Inheritance, and Interfaces[cite: 1].
  - Git: `clone`, `status`, `add`, `commit`, `push`, `pull`, branch management, and Pull Requests (PRs)[cite: 1].
- **Practice Models:** `User`, `Ticket`, and an `IEntity` contract[cite: 1].
- **Deliverable:** Working console application implementing `IEntity`, submitted via branch and Pull Request[cite: 1].

---

## 📅 Week 2: SQL + Database Design
- **Objective:** Learn relational database modeling, normalization, and raw SQL scripting[cite: 2].
- **Key Concepts:**
  - DQL & DML: `SELECT`, `WHERE`, `ORDER BY`, `INSERT`, `UPDATE`, `DELETE`, and aggregations (`GROUP BY`, `HAVING`)[cite: 2].
  - Keys & Constraints: Primary Keys, Foreign Keys, and table relationships ($1:N$, $M:N$)[cite: 2].
  - Multi-table joins: `INNER JOIN` and `LEFT JOIN`[cite: 2].
- **Core Entities:** `User`, `Role`, `Ticket`, `TicketComment`, and `Category`[cite: 2].
- **Deliverable:** Full TicketDesk ERD diagram + executable `schema.sql` (table DDL) and `seed.sql` (starter data)[cite: 2].

---

## 📅 Week 3: EF Core — Domain, DAL & Migrations
- **Objective:** Scaffold a Clean Architecture solution and generate the database using Code-First EF Core[cite: 3].
- **Key Concepts:**
  - Layered project separation: `.Domain` (POCOs), `.DAL` (data access), `.BL` (business logic), `.API`, and `.Shared`[cite: 3].
  - Entity modeling: POCO entities, bidirectional navigation properties, and `TicketStatus` enum[cite: 3].
  - EF Core Configuration: `DbContext`, `DbSet<T>`, and Fluent API constraints inside `OnModelCreating`[cite: 3].
  - CLI Migrations: `dotnet ef migrations add`, `dotnet ef database update`, and model data seeding via `.HasData()`[cite: 3].
- **Deliverable:** Working SQL Server database generated purely from EF Core migrations matching the Week 2 ERD[cite: 3].

---

## 📅 Week 4: Clean Architecture Web API + Full CRUD
- **Objective:** Build a robust, layered ASP.NET Core RESTful Web API with decoupled data access[cite: 4].
- **Key Concepts:**
  - Dependency Injection (DI) and Clean Architecture inward dependency rules[cite: 4].
  - Generic Repository Pattern: `IGenericRepository<T>` and `GenericRepository<T>` in `.DAL`[cite: 4].
  - Service Layer: `TicketService` in `.BL` handling business rules and AutoMapper DTO mappings (`TicketDto`, `CreateTicketDto`)[cite: 4].
  - Controllers: Thin `TicketsController` exposing asynchronous CRUD actions returning proper HTTP status codes (`200`, `201`, `204`, `404`)[cite: 4].
  - API Versioning: URL-segment versioning (`/api/v1/tickets`) documented via Swagger[cite: 4].
- **Deliverable:** Fully functional, versioned CRUD Web API tested and verified against the seeded database[cite: 4].

---

## 📅 Week 5: React Foundations + Ticket UI
- **Objective:** Build a React client application using Vite and integrate it with the Week 4 REST API[cite: 5].
- **Key Concepts:**
  - React Core: JSX, components, unidirectional data flow with `props`, and reactive state with `useState`[cite: 5].
  - API Integration: Asynchronous HTTP requests using `fetch` or `axios` inside `useEffect`[cite: 5].
  - Backend CORS: Configuring `app.UseCors()` in ASP.NET Core to allow frontend requests[cite: 5].
  - Forms & State: Controlled form inputs for creating tickets, list rendering via `.map()`, and unique `key` props[cite: 5].
  - Architecture & Routing: Centralized `api.js` client module and multi-page routing via React Router[cite: 5].
- **Deliverable:** Routed React SPA that lists, views, and creates tickets against the live backend API[cite: 5].
