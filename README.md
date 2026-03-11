# ReGuestSiteGreator

A .NET 10 REST API for managing WordPress, Typo3 and other CMS projects.

## Architecture

The solution uses Clean Architecture with 4 projects:

| Project | Description |
|---|---|
| `ReGuestSiteGreator.Domain` | Domain entities and enums |
| `ReGuestSiteGreator.Application` | DTOs and service interfaces (business contracts) |
| `ReGuestSiteGreator.Infrastructure` | EF Core with PostgreSQL, service implementations |
| `ReGuestSiteGreator.API` | ASP.NET Core Web API controllers |

## Domain Model

- **User** – Authenticated identity with a role (`Admin` or `Partner`)
- **Partner** – A partner record linked to a User and optionally a Plan
- **Plan** – Predefined subscription tier (`Basic`, `Business`, `Premium`)
- **Sitemap** – A collection of pages assigned to a Plan
- **Page** – A web page in a Sitemap (name, slug, title, SEO meta, status, etc.)
- **Block** – Reusable template unit (template, style, script, meta JSON, defaultData JSON)
- **PageBlock** – Join table linking Pages to their Blocks

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 16+](https://www.postgresql.org/) (or Docker)

## Getting Started

### 1. Start PostgreSQL

```bash
docker compose up postgres -d
```

Or configure an existing PostgreSQL instance in `appsettings.json`.

### 2. Apply database migrations

```bash
cd src/ReGuestSiteGreator.API
dotnet ef database update --project ../ReGuestSiteGreator.Infrastructure
```

Alternatively, add this to `Program.cs` for automatic migration on startup:
```csharp
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();
```

### 3. Configure JWT secret

Edit `appsettings.json` or set environment variables:

```json
{
  "Jwt": {
    "Key": "YOUR_STRONG_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "ReGuestSiteGreator",
    "Audience": "ReGuestSiteGreatorClients",
    "ExpiryHours": "24"
  }
}
```

### 4. Run the API

```bash
cd src/ReGuestSiteGreator.API
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

## Default Admin Account

After running migrations, a seeded admin account is available:

| Field | Value |
|---|---|
| Username | `admin` |
| Password | `Admin@123` |

## API Endpoints

### Auth

| Method | Path | Description |
|---|---|---|
| POST | `/api/auth/login` | Login and receive a JWT token |

**Request body:**
```json
{ "username": "admin", "password": "Admin@123" }
```

**Response:**
```json
{
  "token": "<JWT>",
  "expiresAt": "2024-01-02T00:00:00Z",
  "role": "Admin",
  "userId": "00000000-0000-0000-0000-000000000001"
}
```

---

### Admin (requires `Admin` JWT)

| Method | Path | Description |
|---|---|---|
| GET | `/api/admin/partners` | List all partners |
| POST | `/api/admin/partners` | Create a new partner |
| GET | `/api/admin/plans` | List available plans |
| POST | `/api/admin/partners/{partnerId}/assign-plan` | Assign a plan to a partner |

---

### Partner (requires `Partner` JWT)

| Method | Path | Description |
|---|---|---|
| GET | `/api/partner/sitemap` | Get the sitemap for the authenticated partner's plan |
| GET | `/api/partner/pages/{pageId}` | Get full page details (with blocks) |
| GET | `/api/partner/blocks?page=1&pageSize=20` | Paginated list of blocks in the partner's sitemap |

## Docker

Run the full stack (API + PostgreSQL):

```bash
docker compose up
```

