# GitHub Copilot Instructions

## Project Overview

**ReGuestSiteGreator** is a **.NET 10 REST API** for managing CMS projects (WordPress, Typo3, and others).
It exposes endpoints that let an **Admin** manage partners and subscription plans, and let a **Partner** browse their assigned sitemap, pages, and reusable blocks.

---

## Architecture — Clean Architecture (4 layers)

| Project | Role |
|---|---|
| `ReGuestSiteGreator.Domain` | Domain entities and enums. No dependencies on other layers. |
| `ReGuestSiteGreator.Application` | DTOs, service interfaces, and shared helpers (business contracts). Depends only on Domain. |
| `ReGuestSiteGreator.Infrastructure` | EF Core + PostgreSQL data access, service implementations. Depends on Application and Domain. |
| `ReGuestSiteGreator.API` | ASP.NET Core Web API controllers, JWT setup, DI composition root. Depends on Infrastructure. |

---

## Domain Model

| Entity | Description |
|---|---|
| `User` | Authenticated identity with a `Role` (`Admin` or `Partner`), email and password hash. |
| `Partner` | A partner record linked 1-to-1 with a `User`. Optionally assigned a `Plan`. |
| `Plan` | Predefined subscription tier (`Basic`, `Business`, `Premium`). Owns a `Sitemap`. |
| `Sitemap` | A collection of `Page` records, assigned to a `Plan`. |
| `Page` | A web page inside a `Sitemap` (name, slug, title, SEO meta, `PageStatus`). |
| `Block` | Reusable template unit with HTML template, CSS style, JS script, meta JSON and defaultData JSON. |
| `PageBlock` | Many-to-many join table linking `Page` to its `Block` records. |

`PageStatus` enum: `Draft`, `Published`, `Archived`.

---

## Technology Stack

- **Runtime**: .NET 10
- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core with PostgreSQL (Npgsql)
- **Auth**: JWT Bearer tokens (HS256)
- **Database**: PostgreSQL 16+
- **Containerisation**: Docker / Docker Compose

---

## Authentication & Authorisation

- All secure endpoints require a JWT `Bearer` token in the `Authorization` header.
- Two roles are enforced via `[Authorize(Roles = "Admin")]` / `[Authorize(Roles = "Partner")]`.
- Tokens are obtained via `POST /api/auth/login`.

---

## API Endpoints

### Auth
| Method | Path | Description |
|---|---|---|
| POST | `/api/auth/login` | Returns a JWT token |

### Admin _(requires Admin JWT)_
| Method | Path | Description |
|---|---|---|
| GET | `/api/admin/partners` | List all partners |
| POST | `/api/admin/partners` | Create a new partner |
| GET | `/api/admin/plans` | List available plans |
| POST | `/api/admin/partners/{partnerId}/assign-plan` | Assign a plan to a partner |

### Partner _(requires Partner JWT)_
| Method | Path | Description |
|---|---|---|
| GET | `/api/partner/sitemap` | Get the sitemap for the authenticated partner's plan |
| GET | `/api/partner/pages/{pageId}` | Get full page details (with blocks) |
| GET | `/api/partner/blocks` | Paginated list of blocks (`?page=1&pageSize=20`) |

---

## Key Conventions

- **Controllers** live in `src/ReGuestSiteGreator.API/Controllers/`.
- **Service interfaces** live in `src/ReGuestSiteGreator.Application/Interfaces/` and are implemented in `src/ReGuestSiteGreator.Infrastructure/`.
- **DTOs** live in `src/ReGuestSiteGreator.Application/DTOs/`.
- Paged results use the generic `PagedResult<T>` class in `ReGuestSiteGreator.Application.Common`.
- EF Core migrations live in `src/ReGuestSiteGreator.Infrastructure/Data/Migrations/`.
- DI registration for the infrastructure layer is done via `AddInfrastructure(IConfiguration)` extension method.
- The solution file is `ReGuestSiteGreator.slnx` at the repository root.

---

## Local Development

```bash
# 1. Start PostgreSQL
docker compose up postgres -d

# 2. Apply EF Core migrations
cd src/ReGuestSiteGreator.API
dotnet ef database update --project ../ReGuestSiteGreator.Infrastructure

# 3. Run the API
dotnet run
# Available at https://localhost:5001
```

Default seeded admin account: `admin@reguestsitecreator.com` / `Admin@123`

---

## Docker

```bash
# Full stack (API + PostgreSQL)
docker compose up
```

---

## Coding Guidelines for Copilot

- Follow the existing **Clean Architecture** layer boundaries: controllers call services via interfaces; no direct DB access in controllers.
- New domain entities go in `ReGuestSiteGreator.Domain/Entities/`, new enums in `ReGuestSiteGreator.Domain/Enums/`.
- New service contracts (interfaces + DTOs) go in `ReGuestSiteGreator.Application/`.
- Implementations go in `ReGuestSiteGreator.Infrastructure/Services/`.
- New API endpoints go in an appropriate controller in `ReGuestSiteGreator.API/Controllers/`.
- Use `async`/`await` throughout. All service methods should return `Task<T>`.
- Use `Guid` for all entity primary keys.
- EF Core fluent configuration is preferred over data annotations.
- Generate a new EF Core migration after any entity/schema change:
  ```bash
  dotnet ef migrations add <MigrationName> --project src/ReGuestSiteGreator.Infrastructure --startup-project src/ReGuestSiteGreator.API
  ```
