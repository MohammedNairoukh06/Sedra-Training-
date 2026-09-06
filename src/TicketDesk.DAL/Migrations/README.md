# EF Core Migrations

This folder contains the Week 3 EF Core migrations for TicketDesk.

## Migration order

1. `20260906090000_InitialCreate` — creates the database schema (tables, primary keys, foreign keys, indexes, and the TicketCategories many-to-many join table).
2. `20260906090100_SeedRolesAndCategories` — inserts the starter Roles and Categories required by Day 5.

## Apply the migrations

From the solution root:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project src/TicketDesk.DAL --startup-project src/TicketDesk.API
```

The connection string is read from `src/TicketDesk.API/appsettings.json`.

## Important

The migrations are source code and should be committed to Git as part of the Week 3 Pull Request.
