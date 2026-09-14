# Week 3 — EF Core: Domain + DAL + Migrations

**Trainee Training Plan (8-Week Program) — Week 3 of 8**[cite: 2]  
* **Project:** TicketDesk Project (5 working days)[cite: 2]
* **End-of-Week Deliverable:** A working TicketDesk database generated from EF Core migrations with the Domain entities living in `TicketDesk.Domain` and the `DbContext` in `TicketDesk.DAL`[cite: 2].
* **Goal:** The trainee turns last week's ERD into real C# entities and lets EF Core build the exact same database automatically — no hand-written `CREATE TABLE` this time[cite: 2].
* **Note:** The real project starts this week[cite: 2]. Weeks 1–2 were fundamentals and warm-ups[cite: 2]. From now on, everything is real project code in the practice repo, using the same layered structure your team uses in production (`.Domain`, `.Shared`, `.DAL`, `.BL`, `.API`)[cite: 2]. The tables EF Core generates must match the Week 2 ERD exactly[cite: 2].

---

## Starting Point (End of Week 2)
* C# + OOP basics and a working Git flow (`branch` $\rightarrow$ `commit` $\rightarrow$ `push` $\rightarrow$ `Pull Request`) — from Week 1[cite: 2].
* A finished TicketDesk ERD covering all five entities (`User`, `Role`, `Category`, `Ticket`, `TicketComment`) with every PK/FK marked[cite: 2].
* A working SQL script (`schema.sql` + `seed.sql`) that builds the tables and inserts sample data[cite: 2].
* A shared repo where each feature went through its own branch + PR[cite: 2].
* *If any of these is missing, close the gap on Day 1 before starting EF Core — the ERD especially, since it is the blueprint for this whole week[cite: 2].*

---

## Daily Tasks & Schedule

### Day 1 Task: Solution Structure + Domain Entities

**Learn**[cite: 2]
* What each layer is for: `Domain` (entities), `Shared` (DTOs/enums), `DAL` (data access), `BL` (logic), `API` (endpoints)[cite: 2].
* The dependency direction: inner layers know nothing about outer ones; `Domain` references nothing[cite: 2].
* What a POCO entity is: a plain C# class that maps to a table (no database code inside it)[cite: 2].

**Resources**[cite: 2]
* [Create solutions & projects (CLI)](https://learn.microsoft.com/dotnet/core/tools/dotnet-new)[cite: 2]
* [Common web app architectures](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)[cite: 2]
* [EF Core overview](https://learn.microsoft.com/ef/core)[cite: 2]

**Practice**[cite: 2]
* Create the solution and the five projects (`dotnet new sln`, then a `classlib`/`webapi` per layer)[cite: 2].
* Wire the project references in the right direction (`.DAL` $\rightarrow$ `.Domain`, `.BL` $\rightarrow$ `.DAL`, etc.)[cite: 2].
* Write the five Domain entity classes as plain POCOs matching the Week 2 ERD properties only, no logic yet[cite: 2].

**End of Day:** The solution builds, the five projects reference each other correctly, and the Domain entities exist as plain classes[cite: 2].

---

### Day 2 Task: Relationships, Keys & Enums

**Learn**[cite: 2]
* Primary-key conventions (an `Id` property becomes the PK automatically)[cite: 2].
* Foreign keys + navigation properties: how a `Ticket` points to its `User`, and a `User` holds a list of `Tickets`[cite: 2].
* One-to-many (`User` $\rightarrow$ `Tickets`, `Ticket` $\rightarrow$ `Comments`) and many-to-many (`Ticket` $\leftrightarrow$ `Category`)[cite: 2].
* Modeling status as an enum: `Open`, `InProgress`, `Resolved`, `Closed`[cite: 2].

**Resources**[cite: 2]
* [Relationships](https://learn.microsoft.com/ef/core/modeling/relationships)[cite: 2]
* [Keys](https://learn.microsoft.com/ef/core/modeling/keys)[cite: 2]
* [EF Core tutorial (entities & relations)](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)[cite: 2]

**Practice**[cite: 2]
* Add navigation properties in both directions for every relationship in the ERD[cite: 2].
* Add the `TicketStatus` enum and use it on the `Ticket` entity[cite: 2].
* Set up the many-to-many between `Ticket` and `Category` (navigation collections on both sides)[cite: 2].

**End of Day:** The entities express every relationship from the Week 2 ERD through navigation properties[cite: 2].

---

### Day 3 Task: The DbContext + DI — The Core Day

**Learn**[cite: 2]
* What a `DbContext` is and what `DbSet<T>` represents (one per table)[cite: 2].
* Installing the EF Core packages: `Microsoft.EntityFrameworkCore.SqlServer`, `Design`, `Tools`[cite: 2].
* The connection string, and registering the context with DI via `AddDbContext`[cite: 2].
* Fluent API in `OnModelCreating`: configuring relationships and constraints in code[cite: 2].

**Resources**[cite: 2]
* [DbContext configuration](https://learn.microsoft.com/ef/core/dbcontext-configuration)[cite: 2]
* [Connection strings](https://learn.microsoft.com/ef/core/miscellaneous/connection-strings)[cite: 2]
* [Creating & configuring a model (Fluent API)](https://learn.microsoft.com/ef/core/modeling)[cite: 2]

**Practice**[cite: 2]
* Create `TicketDeskDbContext` in `.DAL` with a `DbSet` for each entity[cite: 2].
* Configure the relationships and the many-to-many join in `OnModelCreating`[cite: 2].
* Add the connection string to `appsettings.json` and register the context with DI[cite: 2].

**End of Day:** The `DbContext` compiles with all `DbSets` and the Fluent configuration in place[cite: 2].

---

### Day 4 Task: Migrations — Generating the Database — The Core Day

**Learn**[cite: 2]
* What a migration is: a versioned C# description of a schema change[cite: 2].
* The migration file: the `Up()` (apply) and `Down()` (revert) methods[cite: 2].
* `dotnet ef migrations add` to create one, `dotnet ef database update` to apply it[cite: 2].
* Migrations live in `.DAL` and are committed to the repo like any other code[cite: 2].

**Resources**[cite: 2]
* [Migrations overview](https://learn.microsoft.com/ef/core/managing-schemas/migrations)[cite: 2]
* [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet)[cite: 2]
* [Applying migrations](https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying)[cite: 2]

**Practice**[cite: 2]
* Install the EF tool (`dotnet tool install --global dotnet-ef`)[cite: 2].
* Add the first migration (`InitialCreate`) and apply it to the database[cite: 2].
* Open the database in SSMS / Azure Data Studio and confirm the tables, keys, and relationships match the ERD[cite: 2].

**End of Day:** A real database is created from migrations, and its tables/keys/relationships match the Week 2 ERD[cite: 2].

---

### Day 5 Task: Seeding, Verify & Handover

**Learn**[cite: 2]
* Seeding starter data with `HasData` (roles and categories) inside `OnModelCreating`[cite: 2].
* Adding a second migration for the seed and re-running database update[cite: 2].
* Reading data back with a small LINQ query to prove the model works end-to-end[cite: 2].

**Resources**[cite: 2]
* [Data seeding](https://learn.microsoft.com/ef/core/modeling/data-seeding)[cite: 2]
* [Querying data (LINQ)](https://learn.microsoft.com/ef/core/querying)[cite: 2]
* [Full first-app walkthrough](https://learn.microsoft.com/ef/core/get-started/overview/first-app)[cite: 2]

**Practice**[cite: 2]
* Seed the roles and categories with `HasData`, add the migration, and apply it[cite: 2].
* Verify the seed data landed in the database and run one small query to read it back[cite: 2].
* Tidy up, commit, and open the final Pull Request of the week (`branch` $\rightarrow$ `commit` $\rightarrow$ `push` $\rightarrow$ `PR`)[cite: 2].

**End of Day:** A TicketDesk database built entirely from EF Core migrations with seed data, matching the ERD, pushed via a clean PR[cite: 2].

---

> **Tip:** Keep every feature on its own branch + PR, exactly like Weeks 1–2[cite: 2]. Same repo, new folder (e.g. `/src` for the solution) — so the trainee keeps practicing the Git flow while the real project takes shape[cite: 2].

---

## Notes for the Trainer
* This is the first week of real project code; expect it to feel slower than Weeks 1–2[cite: 2]. That is normal; protect the time[cite: 2].
* Insist the generated database matches the Week 2 ERD exactly — same tables, keys, and relationships[cite: 2].
* No hand-written `CREATE TABLE` this week; the whole point is that EF Core generates the schema from the entities[cite: 2].
* Watch the layer dependency direction: `Domain` must reference nothing; data access stays in `.DAL`[cite: 2].
* Migrations must be committed to the repo; treat them as first-class code in the PR review[cite: 2].
* Git stays mandatory: every feature goes through a branch + PR[cite: 2].
* Week 3 is inside the protected Weeks 1–5 range — if the trainee falls behind, trim Week 7 before touching this[cite: 2].
* If the trainee finishes early: add 2–3 LeetCode Easy problems (not a priority)[cite: 2].
  # Week 4: Real API — Clean Architecture + Full CRUD

**Trainee Training Plan (8-Week Program) — Week 4 of 8**  
* **Project:** TicketDesk Project (5 working days, backend only)[cite: 3]
* **End-of-Week Deliverable:** A fully working, versioned TicketDesk Web API built on Clean Architecture — Repository + Service + Dependency Injection + AutoMapper exposing complete CRUD for tickets over the Week 3 database[cite: 3].
* **Goal:** The trainee builds the real backend the right way: layered, testable, and pattern-based — not logic dumped in a controller[cite: 3].
* **Note:** This is an intensive week (two phases merged)[cite: 3]. It combines the API foundation (Clean Architecture + Repository/Service/DI) and the full CRUD layer (AutoMapper + versioning) into one week[cite: 3]. Backend only — React is parked for now[cite: 3]. Each day is one ordered task that builds directly on the day before, so nobody should skip ahead[cite: 3].

---

## Starting Point (End of Week 3)
* The five Domain entities as POCOs in `TicketDesk.Domain`, matching the ERD[cite: 3].
* `TicketDeskDbContext` in `TicketDesk.DAL` with all DbSets and Fluent configuration[cite: 3].
* EF Core migrations that build a real database, plus seed data (roles, categories) already in it[cite: 3].
* The solution with correct layer references (`.DAL` $\rightarrow$ `.Domain`, etc.) and each feature on its own branch + PR[cite: 3].
* *If the seeded database is not running, fix that on Day 1 before writing any API code — every endpoint this week reads from it[cite: 3].*

---

## Daily Tasks & Schedule

### Day 1 Task: Stand Up the API + Wire Clean Architecture

**Learn**[cite: 3]
* The ASP.NET Core Web API project and the request pipeline (`Program.cs`, middleware, Swagger)[cite: 3].
* The Clean Architecture dependency rule: dependencies point inward (`.API` $\rightarrow$ `.BL` $\rightarrow$ `.DAL` $\rightarrow$ `.Domain`)[cite: 3].
* Registering services in the DI container (the `builder.Services` collection)[cite: 3].

**Resources**[cite: 3]
* [Build your first Web API](https://learn.microsoft.com/aspnet/core/tutorials/first-web-api)[cite: 3]
* [Dependency injection in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection)[cite: 3]
* [Architectural principles (Clean Architecture)](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/architectural-principles)[cite: 3]

**Practice**[cite: 3]
* Confirm `TicketDesk.API` runs and Swagger opens on launch[cite: 3].
* Register `TicketDeskDbContext` in `Program.cs` via `AddDbContext` using the connection string[cite: 3].
* Add a tiny health endpoint (e.g. `GET /api/health`) to prove the pipeline works[cite: 3].

**End of Day:** The API runs, Swagger opens, and the DbContext resolves through DI[cite: 3].

---

### Day 2 Task: Repository Pattern in the DAL

**Learn**[cite: 3]
* Why the Repository pattern: abstract data access behind an interface, keep EF Core out of the business layer[cite: 3].
* A generic repository: `IGenericRepository<T>` with `GetAll`, `GetById`, `Add`, `Update`, `Delete`[cite: 3].
* Interface vs implementation, and registering both with DI (`AddScoped`)[cite: 3].

**Resources**[cite: 3]
* [Persistence layer & repositories](https://learn.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)[cite: 3]
* [Repository pattern in C#](https://dotnettutorials.net/lesson/repository-design-pattern-csharp)[cite: 3]
* [Generics in C#](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/generics)[cite: 3]

**Practice**[cite: 3]
* Create `IGenericRepository<T>` and `GenericRepository<T>` in DAL using the DbContext[cite: 3].
* Register the repository in DI and inject it somewhere temporary to read the seeded categories[cite: 3].
* Confirm the repo returns real rows from the Week 3 database[cite: 3].

**End of Day:** A generic repository resolves via DI and reads seeded data from the real database[cite: 3].

---

### Day 3 Task: Service Layer + DTOs + AutoMapper

**Learn**[cite: 3]
* The Service (business logic) layer: the flow is `Controller` $\rightarrow$ `Service` $\rightarrow$ `Repository`; logic lives in the service[cite: 3].
* DTOs vs entities — never expose entities directly; use `TicketDto`, `CreateTicketDto`, `UpdateTicketDto`[cite: 3].
* AutoMapper: mapping entity $\leftrightarrow$ DTO with a profile instead of copying properties by hand[cite: 3].

**Resources**[cite: 3]
* [Business logic layer](https://dotnettutorials.net/lesson/business-logic-layer-in-asp-net-core)[cite: 3]
* [DTOs in Web API](https://dotnettutorials.net/lesson/dto-in-asp-net-core-web-api)[cite: 3]
* [AutoMapper getting started](https://docs.automapper.org/en/stable/Getting-started.html)[cite: 3]

**Practice**[cite: 3]
* Create `ITicketService` + `TicketService` in `.BL`; define the ticket DTOs in `.Shared`[cite: 3].
* Add an AutoMapper profile and register it in DI; map `Ticket` $\leftrightarrow$ `TicketDto`[cite: 3].
* The service uses the repository and returns DTOs (not entities)[cite: 3].

**End of Day:** The service returns mapped DTOs for tickets, with AutoMapper doing the conversion[cite: 3].

---

### Day 4 Task: Full CRUD Endpoints (Thin Controller)

**Learn**[cite: 3]
* The thin controller principle: no logic in the controller — it only depends on `ITicketService`[cite: 3].
* The five actions: `GET` all, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`[cite: 3].
* Proper responses and `async`/`await`: `Ok`, `NotFound`, `CreatedAtAction`, `NoContent`[cite: 3].

**Resources**[cite: 3]
* [Controller actions & return types](https://learn.microsoft.com/aspnet/core/web-api/action-return-types)[cite: 3]
* [Attribute routing](https://learn.microsoft.com/aspnet/core/mvc/controllers/routing)[cite: 3]
* [Async programming in C#](https://learn.microsoft.com/dotnet/csharp/asynchronous-programming)[cite: 3]

**Practice**[cite: 3]
* Build `TicketsController` with all five CRUD actions calling the service[cite: 3].
* Test each one in Swagger against the seeded database; confirm a created ticket lands in the DB[cite: 3].
* Keep the controller thin — if logic creeps in, push it down into the service[cite: 3].

**End of Day:** Full CRUD for tickets works end-to-end through the API, Service, and Repository layers[cite: 3].

---

### Day 5 Task: API Versioning + Verify + Final PR

**Learn**[cite: 3]
* Why API versioning matters, and the URL-segment style: `/api/v1/tickets`[cite: 3].
* Adding the versioning package (`Asp.Versioning.Mvc`) and tagging controllers with a version[cite: 3].
* A final walk through the full Clean Architecture flow, layer by layer[cite: 3].

**Resources**[cite: 3]
* [API versioning (guide)](https://dotnettutorials.net/lesson/web-api-versioning)[cite: 3]
* [Asp.Versioning docs](https://github.com/dotnet/aspnet-api-versioning/wiki)[cite: 3]
* [Full Web API walkthrough (CRUD recap)](https://learn.microsoft.com/aspnet/core/tutorials/first-web-api)[cite: 3]

**Practice**[cite: 3]
* Add versioning so the endpoints live under `/api/v1/tickets`[cite: 3].
* Re-test the full CRUD on the versioned routes in Swagger[cite: 3].
* Tidy up, commit, and open the final Pull Request of the week (`branch` $\rightarrow$ `commit` $\rightarrow$ `push` $\rightarrow$ `PR`)[cite: 3].

**End of Day:** A versioned, full-CRUD TicketDesk API on Clean Architecture, verified end-to-end and pushed via a clean PR[cite: 3].

---

> **Tip:** Keep each task on its own branch + PR (e.g. `feature/repository`, `feature/ticket-crud`)[cite: 3]. The days are ordered on purpose — Day 4 needs Day 3, which needs Day 2 — so review each PR before the next task starts[cite: 3].

---

## Notes for the Trainer
* This is the heaviest week so far — it merges two phases (API foundation + full CRUD)[cite: 3]. If the trainee is slower, let versioning (Day 5) slip to early next week rather than skipping the patterns[cite: 3].
* Guard the Clean Architecture direction: Domain references nothing; EF Core stays in DAL; no DbContext in controllers[cite: 3].
* Insist on thin controllers — the moment business logic appears in a controller, move it to the service[cite: 3].
* DTOs are mandatory: entities must never be returned directly from an endpoint[cite: 3].
* Every endpoint must work against the real Week 3 database, not fake in-memory data[cite: 3].
* Git stays mandatory: every task goes through a branch + PR, reviewed in order[cite: 3].
* React is parked — do not let it distract from getting the backend patterns right first[cite: 3].
* If the trainee finishes early: add 2–3 LeetCode Easy problems (not a priority)[cite: 3].
