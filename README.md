# GameStore.Api

A simple CRUD example app built while learning ASP.NET (Minimal APIs + Entity Framework Core + PostgreSQL).

This repository contains a minimal Game Store API to manage games and genres. It was created as a learning project and includes basic Create, Read, Update and Delete operations, database migrations, and Swagger UI for exploration.

## Requirements

- .NET SDK 8.0 or later (the project includes builds for net8.0/net9.0)
- PostgreSQL database
- (Optional) dotnet-ef tool for creating / applying migrations

## Environment

The app reads database connection values from environment variables using DotNetEnv. Place a `.env` file in the `GameStore.Api` folder (or set environment variables) with the following variables:

```
DB_HOST=localhost
DB_PORT=5432
DB_NAME=gamestore
DB_USER=postgres
DB_PASSWORD=yourpassword
```

DotNetEnv will load these automatically when the application starts.

## Quick start (Windows / cmd.exe)

1. Ensure PostgreSQL is running and a database exists (or create one):

```
:: using psql (example)
psql -U postgres -c "CREATE DATABASE gamestore;"
```

2. From repository root, run the app:

```
dotnet run --project "GameStore.Api\GameStore.Api.csproj"
```

The app will start and (by default) expose Swagger at `http://localhost:5298/swagger` (port may differ). Use that page to explore endpoints and try requests.

## Apply migrations

If you need to create or update the database migrations locally, install the EF tools and run migrations:

```
dotnet tool install --global dotnet-ef
cd "GameStore.Api"

:: add a migration (if you change the model)
dotnet ef migrations add <MigrationName> --project "GameStore.Api.csproj" --startup-project "GameStore.Api.csproj"

:: apply migrations to the database
dotnet ef database update --project "GameStore.Api.csproj" --startup-project "GameStore.Api.csproj"
```

Note: This project already contains migrations in the `Migrations/` folder. You usually only need to run `database update` to apply them.

## API examples

Open Swagger at `/swagger` to interactively try the API. Example requests below use `curl`.

Create a game (POST /games):

```
curl -X POST http://localhost:5298/games -H "Content-Type: application/json" -d "{\"title\":\"My Game\",\"description\":\"Fun game\",\"price\":19.99,\"genreId\":1}"
```

Update a game (PUT /games/{id}):

```
curl -X PUT http://localhost:5298/games/1 -H "Content-Type: application/json" -d "{\"title\":\"My Updated Game\",\"description\":\"Updated desc\",\"price\":14.99,\"genreId\":1}"
```

Note: requests must reference valid `genreId` values that exist in the `Genres` table. A common error is attempting to create or update a `Game` with a `genreId` that doesn't exist; that triggers a foreign key constraint error from PostgreSQL (see Troubleshooting below).

## Project structure (important folders)

- `GameStore.Api/Program.cs` - minimal API setup, DB registration, Swagger, and endpoint wiring.
- `GameStore.Api/EndPoints/` - endpoint definitions (CRUD routes for games and related resources).
- `GameStore.Api/Entities/` - EF Core entity classes (Game, Genre).
- `GameStore.Api/Dtos/` - data transfer objects used by the API.
- `GameStore.Api/Data/` - DbContext and migration helpers.
- `GameStore.Api/Migrations/` - EF Core migrations that scaffold database schema.

## Troubleshooting

- "insert or update on table \"Games\" violates foreign key constraint \"FK_Games_Genres_Genreid\""
  - Cause: you're trying to insert/update a `Game` with a `genreId` that doesn't exist in the `Genres` table.
  - Fix: create the Genre first or use a valid `genreId`. Example: POST to `/genres` (if there's an endpoint) or directly insert into DB.

- If the app cannot connect to the database, verify your `.env` values (host/port/user/password) and ensure PostgreSQL accepts connections from your host.

## Next steps / learning ideas

- Add authentication/authorization to protect endpoints.
- Add validation attributes and more robust error handling.
- Add integration tests for endpoints.
- Add Dockerfile and compose setup for local DB + app.

---
This project is for learning purposes and does not include production-ready features like logging, security, or advanced error handling. Use it as a starting point to build your own ASP.NET Minimal API applications!


