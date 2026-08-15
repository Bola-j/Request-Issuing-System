# Request Issuing System

Request Issuing System is an ASP.NET Core MVC application for submitting, tracking, and administering requests. It provides public request management and a session-based administration area where requests can be reviewed and approved or rejected.

## Technology

- .NET 8 and ASP.NET Core MVC
- Entity Framework Core 8 with SQL Server
- Razor views and static web assets
- Repository and service layers for request operations

## Features

- Create, edit, list, and delete requests.
- Track a request's status.
- Admin login protected by an HTTP-only session cookie.
- Admin dashboard and request review workflow.
- Approve or reject submitted requests.

## Run locally

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB or another reachable SQL Server instance

### Configure the database

The default development connection string targets LocalDB. Update `SimpleWebApp/appsettings.Development.json` or provide `ConnectionStrings__DefaultConnection` through user secrets or environment variables when using another SQL Server instance.

### Restore and run

```powershell
dotnet restore SimpleWebApp.slnx
dotnet run --project SimpleWebApp/SimpleWebApp.csproj
```

The application creates its database on startup with Entity Framework Core's `EnsureCreated` workflow.

## Security note

Configure the administrator passcode outside source control for deployed environments. Do not use the development value in `appsettings.json` as a production credential.

## License

No license is currently declared. Treat the source as proprietary unless a license is added.
