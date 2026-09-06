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
