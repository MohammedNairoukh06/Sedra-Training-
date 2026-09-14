# TicketDesk — Week 3 (EF Core: Domain + DAL + Migrations)

This week I moved the TicketDesk schema from raw SQL into EF Core. Instead of
writing CREATE TABLE statements by hand like Week 2, I built the five
entities as plain C# classes and let EF Core generate the database from them
through migrations.
## Project layout

- `TicketDesk.Domain` — the five entity classes and the `TicketStatus` enum.
  This layer doesn't reference anything else, since it shouldn't know
  anything about databases.
- `TicketDesk.Shared` — left empty for now, this is where DTOs will go in a
  later week.
- `TicketDesk.DAL` — the DbContext, all the relationship configuration, and
  the migrations.
- `TicketDesk.BL` — also empty for now, business logic comes later.
- `TicketDesk.API` — the connection string and the startup wiring (DI).

Each layer only references the one below it, so `Domain` stays clean and
`DAL` doesn't leak up into `BL` or `API`.

## Entities and relationships

The five entities match the ERD from Week 2: `User`, `Role`, `Category`,
`Ticket`, `TicketComment`. Each `Id` becomes the primary key automatically,
so no extra attributes were needed there.

For the relationships, I added navigation properties in both directions:
a `Role` holds a list of `Users`, a `User` holds a list of `Tickets`, and a
`Ticket` holds a list of `Comments`. Tickets and Categories are many-to-many,
so both sides hold a collection of the other and EF Core builds a join table
behind the scenes.

Ticket status is modeled as an enum (`Open`, `InProgress`, `Resolved`,
`Closed`) rather than a plain string or int, so invalid values can't sneak
in.

## The DbContext

All of the relationship configuration lives in `OnModelCreating` using the
Fluent API — that's where the many-to-many join table gets named, the
delete behavior gets set for each foreign key (so deleting a Role or User
doesn't wipe out unrelated Tickets), and the starter data for Roles and
Categories gets seeded.

The connection string lives in `appsettings.json`, and the context gets
registered with dependency injection in `Program.cs`.

## Migrations

The DAL now contains two EF Core migrations: `InitialCreate` creates the schema,
and `SeedRolesAndCategories` inserts the starter Roles and Categories. Apply them
with `dotnet ef database update`. After applying the migrations, verify the
tables, keys, and relationships in SSMS against the Week 2 ERD. The API startup
query prints the seeded rows as a quick end-to-end sanity check.

Migration files are committed as normal source code in `TicketDesk.DAL/Migrations`.

## Notes to self

- `TicketComment` has two foreign keys pointing at `User` conceptually (the
  ticket it belongs to, and the user who wrote it), so I had to be careful
  setting up the author side correctly since there's no inverse collection
  needed there.
- Cascade delete is only on `Ticket → Comments`, since comments don't mean
  anything without their ticket. Everything else uses restrict so I don't
  accidentally lose data through an unrelated delete.

# Week 4: Real API — Clean Architecture + Full CRUD

## Overview
* **Program:** Trainee Training Plan (8-Week Program) — Week 4 of 8
* **Project:** TicketDesk Project (5 working days, backend only)
* **Goal:** Build a fully working, versioned TicketDesk Web API structured with Clean Architecture (Repository + Service + Dependency Injection + AutoMapper) exposing full CRUD operations for tickets over the Week 3 database.

---

## Architectural Principles
* **Clean Architecture Flow:** Controllers $\rightarrow$ Services (`.BL`) $\rightarrow$ Repositories (`.DAL`) $\rightarrow$ Entities (`.Domain`).
* **Thin Controllers:** Controllers only coordinate HTTP requests and responses; all business logic lives in the Service layer.
* **DTO Encapsulation:** Entities are never exposed directly to the API consumers; all data mapping occurs via AutoMapper.
* **Real Data:** All operations run against the actual seeded database (no fake/in-memory data).

---

## Daily Schedule & Milestones

### Day 1: Stand Up the API + Wire Clean Architecture
* **Focus:** ASP.NET Core request pipeline, middleware, and Dependency Injection setup.
* **Tasks:**
  * Configure `Program.cs` and verify Swagger runs on startup.
  * Register `TicketDeskDbContext` in the DI container via `AddDbContext`.
  * Add a basic health endpoint (`GET /api/health`) to confirm the pipeline is operational.
* **Deliverable:** The API runs, Swagger opens, and `TicketDeskDbContext` resolves through DI.

### Day 2: Repository Pattern in the DAL
* **Focus:** Data abstraction and decoupling EF Core from the business layer.
* **Tasks:**
  * Create `IGenericRepository<T>` and `GenericRepository<T>` in `.DAL`.
  * Implement base CRUD operations (`GetAll`, `GetById`, `Add`, `Update`, `Delete`).
  * Register the repository in DI (`AddScoped`) and verify it reads seeded database rows.
* **Deliverable:** Generic repository resolves via DI and retrieves data from the database.

### Day 3: Service Layer + DTOs + AutoMapper
* **Focus:** Business logic encapsulation and object mapping.
* **Tasks:**
  * Define DTOs (`TicketDto`, `CreateTicketDto`, `UpdateTicketDto`) in `.Shared`.
  * Implement `ITicketService` and `TicketService` in `.BL`.
  * Configure an AutoMapper profile to map between Domain entities and DTOs.
* **Deliverable:** The Service layer executes business operations and returns mapped DTOs.

### Day 4: Full CRUD Endpoints (Thin Controller)
* **Focus:** RESTful actions, HTTP status codes, and asynchronous actions.
* **Tasks:**
  * Build `TicketsController` with the 5 CRUD endpoints (`GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`).
  * Implement appropriate response types (`Ok`, `NotFound`, `CreatedAtAction`, `NoContent`).
  * Verify end-to-end data creation and persistence via Swagger.
* **Deliverable:** End-to-end CRUD operations work across API $\rightarrow$ Service $\rightarrow$ Repository layers.

### Day 5: API Versioning + Final Verification
* **Focus:** API lifecycle management and production readiness.
* **Tasks:**
  * Install `Asp.Versioning.Mvc` and configure URL-segment versioning (`/api/v1/tickets`).
  * Verify all versioned endpoints via Swagger.
  * Conduct a code cleanup, ensure clean Git history, and submit the final Pull Request.
* **Deliverable:** A fully tested, versioned Clean Architecture Web API merged via PR.
