# 🌿 FiTrack — Personal Finance & Workout Tracker
### Panduan Lengkap Pengembangan Aplikasi Desktop

> **Stack:** .NET 10 · Clean Architecture 4-Layer · Angular 18 · Tailwind CSS · NG-Zorro · PostgreSQL (Docker) · Strava API
> **Primary Color:** `#285A48` (Forest Green Gradient)

---

## 📋 Daftar Isi

1. [Gambaran Umum Aplikasi](#1-gambaran-umum-aplikasi)
2. [Arsitektur Sistem](#2-arsitektur-sistem)
3. [Struktur Project](#3-struktur-project)
4. [Setup Environment](#4-setup-environment)
5. [Backend — Clean Architecture (.NET 10)](#5-backend--clean-architecture-net-10)
6. [Security — Token & Session Manager](#6-security--token--session-manager)
7. [Strava Integration](#7-strava-integration)
8. [Frontend — Angular 18](#8-frontend--angular-18)
9. [UI Design System](#9-ui-design-system)
10. [Database Schema](#10-database-schema)
11. [API Endpoints](#11-api-endpoints)
12. [Docker Setup](#12-docker-setup)
13. [Roadmap Fitur](#13-roadmap-fitur)

---

## 1. Gambaran Umum Aplikasi

**FiTrack** adalah aplikasi desktop all-in-one yang menggabungkan manajemen keuangan pribadi dengan pelacak aktivitas olahraga berbasis data Strava. Semua data berjalan secara lokal dengan keamanan berbasis JWT + Session.

### Fitur Utama

#### 💰 Personal Finance Manager
- Dashboard ringkasan keuangan (pemasukan, pengeluaran, saldo)
- Pencatatan transaksi harian dengan kategori kustom
- Anggaran (budget) per kategori dengan progress bar
- Grafik pengeluaran bulanan & tahunan
- Target tabungan dengan tracking progress
- Reminder tagihan berulang (cicilan, langganan)
- Export laporan ke PDF & Excel
- Multi-rekening (tunai, bank, e-wallet)

#### 🏃 Workout Tracker (Strava Connected)
- Sinkronisasi otomatis aktivitas dari Strava
- Dashboard performa: jarak, durasi, kalori, pace
- Grafik tren mingguan/bulanan
- Personal record (PR) otomatis per jenis aktivitas
- Kalender aktivitas (heatmap gaya GitHub)
- Korelasi keuangan vs aktivitas (lihat pola hidup sehat)
- Rencana latihan manual + target mingguan

#### 🔗 Fitur Integrasi
- Widget "Hari ini": ringkasan keuangan + aktivitas dalam satu halaman
- Analisis: berapa yang dibelanjakan untuk sport vs performa atletik
- Notifikasi reminder: jadwal latihan & jatuh tempo tagihan

---

## 2. Arsitektur Sistem

```
┌─────────────────────────────────────────────────────────┐
│                    FRONTEND (Angular 18)                  │
│          Tailwind CSS + NG-Zorro + Chart.js              │
└────────────────────────┬────────────────────────────────┘
                         │ HTTP/REST + JWT Bearer
┌────────────────────────▼────────────────────────────────┐
│                 BACKEND (.NET 10 Web API)                 │
│                                                          │
│  ┌─────────────┐  ┌───────────────┐  ┌───────────────┐  │
│  │ Presentation│  │  Application  │  │    Domain     │  │
│  │   Layer     │→ │    Layer      │→ │    Layer      │  │
│  │ Controllers │  │ Use Cases /   │  │  Entities /   │  │
│  │ Middleware  │  │ Services /    │  │  Interfaces / │  │
│  │ Filters     │  │ DTOs / CQRS  │  │  Value Obj.   │  │
│  └─────────────┘  └───────────────┘  └───────┬───────┘  │
│                                               │          │
│  ┌────────────────────────────────────────────▼───────┐  │
│  │              Infrastructure Layer                   │  │
│  │   EF Core / Repositories / Strava Client /         │  │
│  │   Email / File Storage / Cache (Redis optional)    │  │
│  └────────────────────────────────────────────────────┘  │
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│              PostgreSQL (Docker Container)                │
└─────────────────────────────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│                   Strava API (External)                   │
│              OAuth 2.0 → Webhook / Polling               │
└─────────────────────────────────────────────────────────┘
```

### Clean Architecture — 4 Layer

| Layer | Project | Tanggung Jawab |
|-------|---------|----------------|
| **Domain** | `FiTrack.Domain` | Entity, Value Object, Domain Events, Interface Repository |
| **Application** | `FiTrack.Application` | Use Cases, CQRS (Commands/Queries), DTOs, Validators |
| **Infrastructure** | `FiTrack.Infrastructure` | EF Core, Repository Impl., Strava Client, JWT, File I/O |
| **Presentation** | `FiTrack.API` | Controllers, Middleware, Filters, Swagger, CORS |

---

## 3. Struktur Project

```
fitrack/
├── backend/
│   ├── FiTrack.sln
│   ├── src/
│   │   ├── FiTrack.Domain/
│   │   │   ├── Entities/
│   │   │   │   ├── User.cs
│   │   │   │   ├── Transaction.cs
│   │   │   │   ├── Account.cs
│   │   │   │   ├── Category.cs
│   │   │   │   ├── Budget.cs
│   │   │   │   ├── SavingGoal.cs
│   │   │   │   ├── RecurringBill.cs
│   │   │   │   ├── Activity.cs
│   │   │   │   └── StravaToken.cs
│   │   │   ├── ValueObjects/
│   │   │   │   ├── Money.cs
│   │   │   │   └── Distance.cs
│   │   │   ├── Interfaces/
│   │   │   │   ├── Repositories/
│   │   │   │   │   ├── ITransactionRepository.cs
│   │   │   │   │   ├── IActivityRepository.cs
│   │   │   │   │   └── IUserRepository.cs
│   │   │   │   └── Services/
│   │   │   │       ├── ITokenService.cs
│   │   │   │       └── IStravaService.cs
│   │   │   ├── Enums/
│   │   │   │   ├── TransactionType.cs
│   │   │   │   └── ActivityType.cs
│   │   │   └── Events/
│   │   │       ├── TransactionCreatedEvent.cs
│   │   │       └── ActivitySyncedEvent.cs
│   │   │
│   │   ├── FiTrack.Application/
│   │   │   ├── Features/
│   │   │   │   ├── Finance/
│   │   │   │   │   ├── Commands/
│   │   │   │   │   │   ├── CreateTransaction/
│   │   │   │   │   │   │   ├── CreateTransactionCommand.cs
│   │   │   │   │   │   │   └── CreateTransactionHandler.cs
│   │   │   │   │   │   └── UpdateBudget/
│   │   │   │   │   └── Queries/
│   │   │   │   │       ├── GetDashboardSummary/
│   │   │   │   │       └── GetTransactionList/
│   │   │   │   ├── Workout/
│   │   │   │   │   ├── Commands/
│   │   │   │   │   │   └── SyncStravaActivities/
│   │   │   │   │   └── Queries/
│   │   │   │   │       ├── GetActivityStats/
│   │   │   │   │       └── GetPersonalRecords/
│   │   │   │   └── Auth/
│   │   │   │       ├── Commands/
│   │   │   │       │   ├── Login/
│   │   │   │       │   ├── Register/
│   │   │   │       │   └── RefreshToken/
│   │   │   │       └── Queries/
│   │   │   ├── DTOs/
│   │   │   │   ├── Finance/
│   │   │   │   └── Workout/
│   │   │   ├── Validators/
│   │   │   └── Mappings/
│   │   │       └── MappingProfile.cs
│   │   │
│   │   ├── FiTrack.Infrastructure/
│   │   │   ├── Persistence/
│   │   │   │   ├── AppDbContext.cs
│   │   │   │   ├── Configurations/
│   │   │   │   │   ├── TransactionConfiguration.cs
│   │   │   │   │   └── ActivityConfiguration.cs
│   │   │   │   ├── Repositories/
│   │   │   │   │   ├── TransactionRepository.cs
│   │   │   │   │   └── ActivityRepository.cs
│   │   │   │   └── Migrations/
│   │   │   ├── Services/
│   │   │   │   ├── TokenService.cs
│   │   │   │   ├── SessionService.cs
│   │   │   │   └── StravaService.cs
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   └── FiTrack.API/
│   │       ├── Controllers/
│   │       │   ├── AuthController.cs
│   │       │   ├── FinanceController.cs
│   │       │   ├── WorkoutController.cs
│   │       │   └── DashboardController.cs
│   │       ├── Middleware/
│   │       │   ├── JwtMiddleware.cs
│   │       │   ├── SessionValidationMiddleware.cs
│   │       │   └── ExceptionMiddleware.cs
│   │       ├── Filters/
│   │       │   └── ApiKeyFilter.cs
│   │       ├── Program.cs
│   │       └── appsettings.json
│   │
│   └── tests/
│       ├── FiTrack.Domain.Tests/
│       └── FiTrack.Application.Tests/
│
├── frontend/
│   └── fitrack-web/
│       ├── src/
│       │   ├── app/
│       │   │   ├── core/
│       │   │   │   ├── guards/
│       │   │   │   │   └── auth.guard.ts
│       │   │   │   ├── interceptors/
│       │   │   │   │   ├── jwt.interceptor.ts
│       │   │   │   │   └── error.interceptor.ts
│       │   │   │   ├── services/
│       │   │   │   │   ├── auth.service.ts
│       │   │   │   │   └── session.service.ts
│       │   │   │   └── models/
│       │   │   ├── features/
│       │   │   │   ├── dashboard/
│       │   │   │   ├── finance/
│       │   │   │   │   ├── transactions/
│       │   │   │   │   ├── budgets/
│       │   │   │   │   └── savings/
│       │   │   │   ├── workout/
│       │   │   │   │   ├── activities/
│       │   │   │   │   ├── stats/
│       │   │   │   │   └── strava-connect/
│       │   │   │   └── settings/
│       │   │   ├── shared/
│       │   │   │   ├── components/
│       │   │   │   │   ├── sidebar/
│       │   │   │   │   ├── stat-card/
│       │   │   │   │   └── chart-wrapper/
│       │   │   │   └── pipes/
│       │   │   └── layout/
│       │   ├── assets/
│       │   └── styles/
│       │       ├── tailwind.css
│       │       └── ng-zorro-theme.less
│       ├── angular.json
│       ├── tailwind.config.js
│       └── package.json
│
├── docker/
│   ├── docker-compose.yml
│   └── postgres/
│       └── init.sql
│
└── docs/
    └── PANDUAN_FITRACK_APP.md  ← (file ini)
```

---

## 4. Setup Environment

### Prasyarat

```bash
# Cek versi yang dibutuhkan
dotnet --version       # >= 10.0
node --version         # >= 20.x
npm --version          # >= 10.x
docker --version       # >= 24.x
docker compose version # >= 2.x
```

### Instalasi Tools

```bash
# .NET 10 SDK
# Download dari: https://dotnet.microsoft.com/download/dotnet/10.0

# Angular CLI 18
npm install -g @angular/cli@18

# EF Core CLI
dotnet tool install --global dotnet-ef

# Buat solution
mkdir fitrack && cd fitrack
dotnet new sln -n FiTrack

# Buat projects
dotnet new classlib -n FiTrack.Domain        -o backend/src/FiTrack.Domain
dotnet new classlib -n FiTrack.Application   -o backend/src/FiTrack.Application
dotnet new classlib -n FiTrack.Infrastructure -o backend/src/FiTrack.Infrastructure
dotnet new webapi   -n FiTrack.API           -o backend/src/FiTrack.API

# Tambahkan ke solution
dotnet sln add backend/src/FiTrack.Domain/FiTrack.Domain.csproj
dotnet sln add backend/src/FiTrack.Application/FiTrack.Application.csproj
dotnet sln add backend/src/FiTrack.Infrastructure/FiTrack.Infrastructure.csproj
dotnet sln add backend/src/FiTrack.API/FiTrack.API.csproj

# Set referensi antar project
dotnet add backend/src/FiTrack.Application/FiTrack.Application.csproj reference \
  backend/src/FiTrack.Domain/FiTrack.Domain.csproj

dotnet add backend/src/FiTrack.Infrastructure/FiTrack.Infrastructure.csproj reference \
  backend/src/FiTrack.Application/FiTrack.Application.csproj

dotnet add backend/src/FiTrack.API/FiTrack.API.csproj reference \
  backend/src/FiTrack.Infrastructure/FiTrack.Infrastructure.csproj
```

### NuGet Packages

```bash
# Application Layer
cd backend/src/FiTrack.Application
dotnet add package MediatR
dotnet add package AutoMapper
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

# Infrastructure Layer
cd ../FiTrack.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package BCrypt.Net-Next
dotnet add package StackExchange.Redis          # opsional, untuk session
dotnet add package Polly                        # retry policy untuk Strava API

# API Layer
cd ../FiTrack.API
dotnet add package Swashbuckle.AspNetCore
dotnet add package Serilog.AspNetCore
dotnet add package AspNetCoreRateLimit
```

### NPM Packages (Angular)

```bash
cd frontend
ng new fitrack-web --routing --style=less --standalone

cd fitrack-web
npm install ng-zorro-antd
npm install chart.js ng2-charts
npm install @angular/cdk
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init
```

---

## 5. Backend — Clean Architecture (.NET 10)

### 5.1 Domain Layer — Entities

```csharp
// FiTrack.Domain/Entities/User.cs
public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? StravaAthleteId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = [];
    public ICollection<Activity> Activities { get; private set; } = [];
    public ICollection<Account> Accounts { get; private set; } = [];
    public StravaToken? StravaToken { get; private set; }

    private User() { }

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void ConnectStrava(string athleteId)
    {
        StravaAthleteId = athleteId;
    }
}
```

```csharp
// FiTrack.Domain/Entities/Transaction.cs
public class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public Account Account { get; private set; }
    public Category Category { get; private set; }

    private Transaction() { }

    public static Transaction Create(
        Guid userId, Guid accountId, Guid categoryId,
        TransactionType type, decimal amount,
        string description, DateTime date, string? notes = null)
    {
        if (amount <= 0) throw new DomainException("Amount must be positive");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            Description = description,
            Date = date,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

```csharp
// FiTrack.Domain/Entities/Activity.cs
public class Activity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public long? StravaActivityId { get; private set; }
    public ActivityType Type { get; private set; }
    public string Name { get; private set; }
    public float DistanceMeters { get; private set; }
    public int DurationSeconds { get; private set; }
    public float? ElevationGainMeters { get; private set; }
    public int? CaloriesBurned { get; private set; }
    public float? AveragePace { get; private set; }
    public float? AverageHeartRate { get; private set; }
    public DateTime StartDate { get; private set; }
    public bool IsFromStrava { get; private set; }

    public User User { get; private set; }

    private Activity() { }

    public static Activity CreateFromStrava(Guid userId, StravaActivityDto dto)
    {
        return new Activity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StravaActivityId = dto.Id,
            Type = Enum.Parse<ActivityType>(dto.Type, true),
            Name = dto.Name,
            DistanceMeters = dto.Distance,
            DurationSeconds = dto.MovingTime,
            ElevationGainMeters = dto.TotalElevationGain,
            CaloriesBurned = dto.Calories,
            AveragePace = dto.Distance > 0 ? dto.MovingTime / (dto.Distance / 1000f) : null,
            StartDate = dto.StartDate,
            IsFromStrava = true
        };
    }
}
```

```csharp
// FiTrack.Domain/Entities/StravaToken.cs
public class StravaToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string AthleteId { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    private StravaToken() { }

    public static StravaToken Create(
        Guid userId, string accessToken,
        string refreshToken, DateTime expiresAt, string athleteId)
    {
        return new StravaToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            AthleteId = athleteId,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Refresh(string newAccessToken, string newRefreshToken, DateTime newExpiresAt)
    {
        AccessToken = newAccessToken;
        RefreshToken = newRefreshToken;
        ExpiresAt = newExpiresAt;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

### 5.2 Application Layer — CQRS

```csharp
// FiTrack.Application/Features/Finance/Commands/CreateTransaction/CreateTransactionCommand.cs
public record CreateTransactionCommand(
    Guid AccountId,
    Guid CategoryId,
    TransactionType Type,
    decimal Amount,
    string Description,
    DateTime Date,
    string? Notes
) : IRequest<TransactionDto>;

// Handler
public class CreateTransactionHandler(
    ITransactionRepository repo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(
        CreateTransactionCommand request,
        CancellationToken ct)
    {
        var transaction = Transaction.Create(
            currentUser.UserId,
            request.AccountId,
            request.CategoryId,
            request.Type,
            request.Amount,
            request.Description,
            request.Date,
            request.Notes
        );

        await repo.AddAsync(transaction, ct);
        await repo.SaveChangesAsync(ct);

        return mapper.Map<TransactionDto>(transaction);
    }
}
```

```csharp
// FiTrack.Application/Features/Finance/Queries/GetDashboardSummary/GetDashboardSummaryQuery.cs
public record GetDashboardSummaryQuery(int Month, int Year) : IRequest<DashboardSummaryDto>;

public class GetDashboardSummaryHandler(
    ITransactionRepository transactionRepo,
    IActivityRepository activityRepo,
    ICurrentUserService currentUser
) : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    public async Task<DashboardSummaryDto> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        var startDate = new DateTime(request.Year, request.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var transactions = await transactionRepo.GetByDateRangeAsync(userId, startDate, endDate, ct);
        var activities = await activityRepo.GetByDateRangeAsync(userId, startDate, endDate, ct);

        return new DashboardSummaryDto
        {
            TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
            TotalActivities = activities.Count,
            TotalDistanceKm = activities.Sum(a => a.DistanceMeters) / 1000f,
            TotalCaloriesBurned = activities.Sum(a => a.CaloriesBurned ?? 0),
            ExpenseByCategory = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryExpenseDto { Category = g.Key, Amount = g.Sum(t => t.Amount) })
                .ToList(),
            ActivityByType = activities
                .GroupBy(a => a.Type.ToString())
                .Select(g => new ActivitySummaryDto { Type = g.Key, Count = g.Count() })
                .ToList()
        };
    }
}
```

### 5.3 Infrastructure Layer — DbContext

```csharp
// FiTrack.Infrastructure/Persistence/AppDbContext.cs
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();
    public DbSet<RecurringBill> RecurringBills => Set<RecurringBill>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<StravaToken> StravaTokens => Set<StravaToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
```

### 5.4 Presentation Layer — Program.cs

```csharp
// FiTrack.API/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR + FluentValidation
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(FiTrack.Application.AssemblyReference).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(FiTrack.Application.AssemblyReference).Assembly);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Rate Limiting
builder.Services.AddRateLimiter(opts =>
{
    opts.AddFixedWindowLimiter("auth", o =>
    {
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromMinutes(1);
    });
});

// CORS (untuk Angular dev)
builder.Services.AddCors(opts =>
    opts.AddPolicy("FiTrackPolicy", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IStravaService, StravaService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpClient<IStravaService, StravaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("FiTrackPolicy");
app.UseAuthentication();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
```

---

## 6. Security — Token & Session Manager

### 6.1 JWT Token Service

```csharp
// FiTrack.Infrastructure/Services/TokenService.cs
public class TokenService(IConfiguration config) : ITokenService
{
    private readonly string _secret = config["Jwt:Secret"]!;
    private readonly string _issuer = config["Jwt:Issuer"]!;
    private readonly string _audience = config["Jwt:Audience"]!;

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username),
            new("stravaConnected", (user.StravaAthleteId != null).ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),  // Access token: 15 menit
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch { return null; }
    }
}
```

### 6.2 Session Manager

```csharp
// FiTrack.Domain/Entities/UserSession.cs
public class UserSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public string DeviceInfo { get; private set; }
    public string IpAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    private UserSession() { }

    public static UserSession Create(
        Guid userId, string refreshToken,
        string deviceInfo, string ipAddress)
    {
        return new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RefreshToken = refreshToken,
            RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)  // Refresh token: 7 hari
        };
    }

    public void Revoke() => RevokedAt = DateTime.UtcNow;
}
```

```csharp
// FiTrack.Infrastructure/Services/SessionService.cs
public class SessionService(AppDbContext db) : ISessionService
{
    public async Task<UserSession> CreateSessionAsync(
        Guid userId, string refreshToken,
        string deviceInfo, string ip,
        CancellationToken ct = default)
    {
        // Hapus session lama jika lebih dari 5 perangkat
        var existingSessions = await db.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync(ct);

        if (existingSessions.Count >= 5)
        {
            existingSessions.First().Revoke();
        }

        var session = UserSession.Create(userId, refreshToken, deviceInfo, ip);
        db.UserSessions.Add(session);
        await db.SaveChangesAsync(ct);
        return session;
    }

    public async Task<UserSession?> ValidateRefreshTokenAsync(
        string refreshToken, CancellationToken ct = default)
    {
        var sessions = await db.UserSessions
            .Include(s => s.User)
            .Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        return sessions.FirstOrDefault(s =>
            BCrypt.Net.BCrypt.Verify(refreshToken, s.RefreshTokenHash));
    }

    public async Task RevokeSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        var session = await db.UserSessions.FindAsync([sessionId], ct);
        session?.Revoke();
        await db.SaveChangesAsync(ct);
    }

    public async Task RevokeAllSessionsAsync(Guid userId, CancellationToken ct = default)
    {
        var sessions = await db.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ToListAsync(ct);

        sessions.ForEach(s => s.Revoke());
        await db.SaveChangesAsync(ct);
    }
}
```

### 6.3 Session Validation Middleware

```csharp
// FiTrack.API/Middleware/SessionValidationMiddleware.cs
public class SessionValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        // Skip untuk endpoint publik
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var sessionId = context.User.FindFirst("sessionId")?.Value;
            if (sessionId != null && Guid.TryParse(sessionId, out var sid))
            {
                var isValid = await sessionService.IsSessionActiveAsync(sid);
                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { message = "Session expired or revoked" });
                    return;
                }
            }
        }

        await next(context);
    }
}
```

### 6.4 Auth Controller

```csharp
// FiTrack.API/Controllers/AuthController.cs
[ApiController]
[Route("api/auth")]
public class AuthController(ISender mediator, ITokenService tokenService, ISessionService sessionService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var user = await mediator.Send(command);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();

        var deviceInfo = Request.Headers.UserAgent.ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var session = await sessionService.CreateSessionAsync(user.Id, refreshToken, deviceInfo, ip);

        // Set refresh token di HttpOnly cookie
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new
        {
            accessToken,
            expiresIn = 900,  // 15 menit dalam detik
            user = new { user.Id, user.Username, user.Email, StravaConnected = user.StravaAthleteId != null }
        });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "No refresh token" });

        var session = await sessionService.ValidateRefreshTokenAsync(refreshToken);
        if (session == null)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        // Rotate refresh token
        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newAccessToken = tokenService.GenerateAccessToken(session.User);

        await sessionService.RotateRefreshTokenAsync(session.Id, newRefreshToken);

        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new { accessToken = newAccessToken, expiresIn = 900 });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken != null)
        {
            var session = await sessionService.ValidateRefreshTokenAsync(refreshToken);
            if (session != null) await sessionService.RevokeSessionAsync(session.Id);
        }

        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logged out successfully" });
    }
}
```

---

## 7. Strava Integration

### 7.1 Setup Strava App

1. Buka [https://www.strava.com/settings/api](https://www.strava.com/settings/api)
2. Buat aplikasi baru:
   - **Application Name:** FiTrack
   - **Category:** Data Importer
   - **Website:** `http://localhost:4200`
   - **Authorization Callback Domain:** `localhost`
3. Catat **Client ID** dan **Client Secret**

### 7.2 OAuth Flow

```
[Angular App]
     │
     │  1. Redirect ke Strava OAuth
     ▼
https://www.strava.com/oauth/authorize
  ?client_id={CLIENT_ID}
  &response_type=code
  &redirect_uri=http://localhost:4200/strava/callback
  &scope=read,activity:read_all
     │
     │  2. User authorize → redirect balik dengan ?code=xxx
     ▼
[Angular Callback Component]
     │
     │  3. POST /api/strava/exchange-token { code }
     ▼
[.NET API → Strava Token Exchange]
     │
     │  4. Simpan access_token + refresh_token ke DB
     │  5. Return sukses
     ▼
[Dashboard menampilkan data Strava]
```

### 7.3 Strava Service

```csharp
// FiTrack.Infrastructure/Services/StravaService.cs
public class StravaService(
    HttpClient httpClient,
    AppDbContext db,
    IConfiguration config
) : IStravaService
{
    private readonly string _clientId = config["Strava:ClientId"]!;
    private readonly string _clientSecret = config["Strava:ClientSecret"]!;

    public async Task<StravaToken> ExchangeCodeAsync(
        Guid userId, string code, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "https://www.strava.com/oauth/token",
            new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                code,
                grant_type = "authorization_code"
            }, ct);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<StravaTokenResponse>(ct: ct);

        var token = StravaToken.Create(
            userId,
            data!.AccessToken,
            data.RefreshToken,
            DateTimeOffset.FromUnixTimeSeconds(data.ExpiresAt).UtcDateTime,
            data.Athlete.Id.ToString()
        );

        db.StravaTokens.Add(token);
        await db.SaveChangesAsync(ct);
        return token;
    }

    public async Task<string> GetValidAccessTokenAsync(
        Guid userId, CancellationToken ct = default)
    {
        var token = await db.StravaTokens
            .FirstOrDefaultAsync(t => t.UserId == userId, ct)
            ?? throw new NotFoundException("Strava not connected");

        if (!token.IsExpired) return token.AccessToken;

        // Auto-refresh token
        var response = await httpClient.PostAsJsonAsync(
            "https://www.strava.com/oauth/token",
            new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                refresh_token = token.RefreshToken,
                grant_type = "refresh_token"
            }, ct);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<StravaTokenResponse>(ct: ct);

        token.Refresh(
            data!.AccessToken,
            data.RefreshToken,
            DateTimeOffset.FromUnixTimeSeconds(data.ExpiresAt).UtcDateTime
        );

        await db.SaveChangesAsync(ct);
        return token.AccessToken;
    }

    public async Task<List<Activity>> SyncActivitiesAsync(
        Guid userId, int page = 1, int perPage = 30,
        CancellationToken ct = default)
    {
        var accessToken = await GetValidAccessTokenAsync(userId, ct);

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var stravaActivities = await httpClient.GetFromJsonAsync<List<StravaActivityDto>>(
            $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}", ct);

        var activities = new List<Activity>();
        foreach (var dto in stravaActivities ?? [])
        {
            var exists = await db.Activities
                .AnyAsync(a => a.StravaActivityId == dto.Id, ct);

            if (!exists)
            {
                var activity = Activity.CreateFromStrava(userId, dto);
                db.Activities.Add(activity);
                activities.Add(activity);
            }
        }

        await db.SaveChangesAsync(ct);
        return activities;
    }
}
```

---

## 8. Frontend — Angular 18

### 8.1 Auth Service dengan Token Refresh Otomatis

```typescript
// src/app/core/services/auth.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

export interface AuthUser {
  id: string;
  username: string;
  email: string;
  stravaConnected: boolean;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly API = 'http://localhost:5000/api';

  private accessToken: string | null = null;
  private tokenExpiry: Date | null = null;

  private currentUserSubject = new BehaviorSubject<AuthUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  get isLoggedIn(): boolean {
    return !!this.accessToken && !!this.tokenExpiry && new Date() < this.tokenExpiry;
  }

  get token(): string | null {
    return this.accessToken;
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.API}/auth/login`, { email, password }, { withCredentials: true })
      .pipe(
        tap(res => this.setToken(res.accessToken, res.expiresIn, res.user)),
        catchError(err => {
          this.clearAuth();
          return throwError(() => err);
        })
      );
  }

  refreshToken(): Observable<any> {
    return this.http.post<any>(`${this.API}/auth/refresh`, {}, { withCredentials: true })
      .pipe(
        tap(res => this.setToken(res.accessToken, res.expiresIn)),
        catchError(err => {
          this.clearAuth();
          this.router.navigate(['/login']);
          return throwError(() => err);
        })
      );
  }

  logout(): void {
    this.http.post(`${this.API}/auth/logout`, {}, { withCredentials: true }).subscribe();
    this.clearAuth();
    this.router.navigate(['/login']);
  }

  private setToken(token: string, expiresIn: number, user?: AuthUser): void {
    this.accessToken = token;
    this.tokenExpiry = new Date(Date.now() + expiresIn * 1000);
    if (user) this.currentUserSubject.next(user);

    // Auto-refresh 1 menit sebelum expire
    const refreshIn = (expiresIn - 60) * 1000;
    setTimeout(() => this.refreshToken().subscribe(), refreshIn);
  }

  private clearAuth(): void {
    this.accessToken = null;
    this.tokenExpiry = null;
    this.currentUserSubject.next(null);
  }
}
```

### 8.2 JWT Interceptor

```typescript
// src/app/core/interceptors/jwt.interceptor.ts
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);

  const addToken = (request: typeof req) => {
    if (!auth.token) return request;
    return request.clone({
      setHeaders: { Authorization: `Bearer ${auth.token}` }
    });
  };

  return next(addToken(req)).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('/auth/')) {
        return auth.refreshToken().pipe(
          switchMap(() => next(addToken(req))),
          catchError(refreshErr => throwError(() => refreshErr))
        );
      }
      return throwError(() => error);
    })
  );
};
```

### 8.3 App Config (Bootstrap)

```typescript
// src/app/app.config.ts
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { NZ_I18N, id_ID } from 'ng-zorro-antd/i18n';
import { routes } from './app.routes';
import { jwtInterceptor } from './core/interceptors/jwt.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([jwtInterceptor, errorInterceptor])),
    provideAnimations(),
    { provide: NZ_I18N, useValue: id_ID },
  ]
};
```

### 8.4 Dashboard Component

```typescript
// src/app/features/dashboard/dashboard.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { DashboardService } from './dashboard.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NzCardModule, NzStatisticModule, NzGridModule, NzSpinModule],
  template: `
    <div class="p-6">
      <h1 class="text-2xl font-medium text-primary-800 mb-6">
        Selamat datang, {{ (currentUser$ | async)?.username }} 👋
      </h1>

      <nz-spin [nzSpinning]="loading">
        <!-- Finance Summary -->
        <div nz-row [nzGutter]="[16, 16]" class="mb-6">
          <div nz-col [nzSpan]="6">
            <nz-card class="stat-card income">
              <nz-statistic
                [nzValue]="summary?.totalIncome | currency:'IDR':'Rp ':'1.0-0'"
                nzTitle="Pemasukan Bulan Ini"
                nzPrefix="↑"
              />
            </nz-card>
          </div>
          <div nz-col [nzSpan]="6">
            <nz-card class="stat-card expense">
              <nz-statistic
                [nzValue]="summary?.totalExpense | currency:'IDR':'Rp ':'1.0-0'"
                nzTitle="Pengeluaran Bulan Ini"
                nzPrefix="↓"
              />
            </nz-card>
          </div>
          <div nz-col [nzSpan]="6">
            <nz-card class="stat-card activity">
              <nz-statistic
                [nzValue]="summary?.totalActivities"
                nzTitle="Aktivitas Bulan Ini"
                nzSuffix="aktivitas"
              />
            </nz-card>
          </div>
          <div nz-col [nzSpan]="6">
            <nz-card class="stat-card distance">
              <nz-statistic
                [nzValue]="summary?.totalDistanceKm | number:'1.1-1'"
                nzTitle="Total Jarak"
                nzSuffix="km"
              />
            </nz-card>
          </div>
        </div>
      </nz-spin>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  protected authService = inject(AuthService);
  protected currentUser$ = this.authService.currentUser$;

  summary: any = null;
  loading = true;

  ngOnInit() {
    const now = new Date();
    this.dashboardService.getSummary(now.getMonth() + 1, now.getFullYear())
      .subscribe({
        next: data => { this.summary = data; this.loading = false; },
        error: () => { this.loading = false; }
      });
  }
}
```

---

## 9. UI Design System

### 9.1 Tailwind Config

```javascript
// tailwind.config.js
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        primary: {
          50:  '#f0f7f4',
          100: '#d9ede6',
          200: '#b3dbd0',
          300: '#7dbfaf',
          400: '#4d9f8b',
          500: '#2d7a64',
          600: '#285A48',   // ← primary brand
          700: '#1f4738',
          800: '#18382c',
          900: '#122b22',
          950: '#0a1a14',
        }
      },
      backgroundImage: {
        // Gradient utama (sidebar, header)
        'gradient-primary': 'linear-gradient(135deg, #285A48 0%, #1a3d31 40%, #0f2820 100%)',
        // Gradient card highlight
        'gradient-card': 'linear-gradient(135deg, #285A48 0%, #3d7a62 100%)',
        // Gradient subtle (background section)
        'gradient-soft': 'linear-gradient(180deg, #f0f7f4 0%, #ffffff 100%)',
        // Gradient income
        'gradient-income': 'linear-gradient(135deg, #285A48 0%, #4d9f8b 100%)',
        // Gradient expense
        'gradient-expense': 'linear-gradient(135deg, #7f1d1d 0%, #dc2626 100%)',
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif'],
      },
      boxShadow: {
        'primary': '0 4px 24px 0 rgba(40, 90, 72, 0.18)',
        'card': '0 2px 12px 0 rgba(0, 0, 0, 0.06)',
      }
    }
  },
  plugins: []
};
```

### 9.2 Global Styles + NG-Zorro Theme

```less
// src/styles/ng-zorro-theme.less
@primary-color: #285A48;
@link-color: #285A48;
@success-color: #285A48;
@info-color: #1890ff;
@warning-color: #faad14;
@error-color: #dc2626;
@border-radius-base: 8px;
@font-family: 'Inter', -apple-system, BlinkMacSystemFont, sans-serif;

// Sidebar
@layout-sider-background: #122b22;
@menu-dark-bg: #122b22;
@menu-dark-item-active-bg: #285A48;
@menu-dark-selected-item-text-color: #ffffff;

// Buttons
@btn-primary-bg: #285A48;
@btn-primary-border: #285A48;
```

```css
/* src/styles/tailwind.css */
@tailwind base;
@tailwind components;
@tailwind utilities;

@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&display=swap');

@layer components {
  /* Stat Cards dengan gradient */
  .stat-card {
    @apply rounded-xl shadow-card border-0 overflow-hidden;
  }
  .stat-card.income {
    background: theme('backgroundImage.gradient-income');
    @apply text-white;
  }
  .stat-card.expense {
    background: theme('backgroundImage.gradient-expense');
    @apply text-white;
  }
  .stat-card.activity {
    @apply bg-primary-50 border border-primary-200;
  }

  /* Sidebar gradient */
  .sidebar-gradient {
    background: theme('backgroundImage.gradient-primary');
  }

  /* Primary button gradient */
  .btn-primary-gradient {
    background: theme('backgroundImage.gradient-card');
    @apply text-white border-0 rounded-lg px-4 py-2
           hover:opacity-90 transition-opacity shadow-primary;
  }

  /* Card dengan left border accent */
  .card-accent {
    @apply bg-white rounded-xl shadow-card border-l-4 border-primary-600;
  }
}
```

### 9.3 Color Palette Visual

| Token | Hex | Penggunaan |
|-------|-----|-----------|
| `primary-600` | `#285A48` | Brand utama, sidebar active |
| `primary-700` | `#1f4738` | Sidebar background |
| `primary-900` | `#122b22` | Sidebar dark |
| `primary-50` | `#f0f7f4` | Background halaman |
| `primary-100` | `#d9ede6` | Card background subtle |
| Gradient main | `#285A48 → #0f2820` | Sidebar, header hero |
| Gradient card | `#285A48 → #3d7a62` | Stat card hijau |

---

## 10. Database Schema

```sql
-- PostgreSQL Schema

-- Users & Auth
CREATE TABLE users (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username    VARCHAR(50) UNIQUE NOT NULL,
    email       VARCHAR(255) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    strava_athlete_id VARCHAR(50),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE user_sessions (
    id                UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id           UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    refresh_token_hash TEXT NOT NULL,
    device_info       TEXT,
    ip_address        VARCHAR(45),
    created_at        TIMESTAMPTZ DEFAULT NOW(),
    expires_at        TIMESTAMPTZ NOT NULL,
    revoked_at        TIMESTAMPTZ
);

CREATE TABLE strava_tokens (
    id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id       UUID UNIQUE NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    access_token  TEXT NOT NULL,
    refresh_token TEXT NOT NULL,
    expires_at    TIMESTAMPTZ NOT NULL,
    athlete_id    VARCHAR(50) NOT NULL,
    updated_at    TIMESTAMPTZ DEFAULT NOW()
);

-- Finance
CREATE TABLE accounts (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name        VARCHAR(100) NOT NULL,
    type        VARCHAR(30) NOT NULL, -- cash, bank, ewallet, investment
    balance     NUMERIC(15,2) DEFAULT 0,
    color       VARCHAR(7),
    icon        VARCHAR(50),
    is_active   BOOLEAN DEFAULT TRUE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE categories (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id     UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name        VARCHAR(100) NOT NULL,
    type        VARCHAR(10) NOT NULL, -- income / expense
    icon        VARCHAR(50),
    color       VARCHAR(7),
    is_default  BOOLEAN DEFAULT FALSE,
    created_at  TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE transactions (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    account_id      UUID NOT NULL REFERENCES accounts(id),
    category_id     UUID NOT NULL REFERENCES categories(id),
    type            VARCHAR(10) NOT NULL, -- income / expense / transfer
    amount          NUMERIC(15,2) NOT NULL,
    description     VARCHAR(255) NOT NULL,
    notes           TEXT,
    date            DATE NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);
CREATE INDEX idx_transactions_user_date ON transactions(user_id, date);

CREATE TABLE budgets (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    category_id     UUID NOT NULL REFERENCES categories(id),
    amount          NUMERIC(15,2) NOT NULL,
    month           SMALLINT NOT NULL, -- 1-12
    year            SMALLINT NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT NOW(),
    UNIQUE(user_id, category_id, month, year)
);

CREATE TABLE saving_goals (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name            VARCHAR(100) NOT NULL,
    target_amount   NUMERIC(15,2) NOT NULL,
    current_amount  NUMERIC(15,2) DEFAULT 0,
    target_date     DATE,
    is_completed    BOOLEAN DEFAULT FALSE,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE recurring_bills (
    id              UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    category_id     UUID REFERENCES categories(id),
    name            VARCHAR(100) NOT NULL,
    amount          NUMERIC(15,2) NOT NULL,
    due_day         SMALLINT NOT NULL, -- 1-31
    is_active       BOOLEAN DEFAULT TRUE,
    last_reminded   TIMESTAMPTZ,
    created_at      TIMESTAMPTZ DEFAULT NOW()
);

-- Workout
CREATE TABLE activities (
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    strava_activity_id  BIGINT UNIQUE,
    type                VARCHAR(50) NOT NULL, -- run, ride, swim, walk, etc.
    name                VARCHAR(255) NOT NULL,
    distance_meters     FLOAT,
    duration_seconds    INT NOT NULL,
    elevation_gain_m    FLOAT,
    calories_burned     INT,
    average_pace        FLOAT, -- sec/km
    average_heart_rate  FLOAT,
    start_date          TIMESTAMPTZ NOT NULL,
    is_from_strava      BOOLEAN DEFAULT FALSE,
    created_at          TIMESTAMPTZ DEFAULT NOW()
);
CREATE INDEX idx_activities_user_date ON activities(user_id, start_date);
```

---

## 11. API Endpoints

### Auth
| Method | Endpoint | Deskripsi | Auth |
|--------|----------|-----------|------|
| POST | `/api/auth/register` | Daftar akun baru | ❌ |
| POST | `/api/auth/login` | Login, dapat JWT + set cookie | ❌ |
| POST | `/api/auth/refresh` | Refresh access token | Cookie |
| POST | `/api/auth/logout` | Logout & revoke session | ✅ |
| GET | `/api/auth/sessions` | Lihat semua sesi aktif | ✅ |
| DELETE | `/api/auth/sessions/{id}` | Revoke sesi tertentu | ✅ |

### Finance
| Method | Endpoint | Deskripsi | Auth |
|--------|----------|-----------|------|
| GET | `/api/finance/dashboard?month=&year=` | Ringkasan dashboard | ✅ |
| GET | `/api/finance/transactions` | Daftar transaksi (filter, page) | ✅ |
| POST | `/api/finance/transactions` | Buat transaksi | ✅ |
| PUT | `/api/finance/transactions/{id}` | Update transaksi | ✅ |
| DELETE | `/api/finance/transactions/{id}` | Hapus transaksi | ✅ |
| GET | `/api/finance/accounts` | Daftar rekening | ✅ |
| POST | `/api/finance/accounts` | Buat rekening | ✅ |
| GET | `/api/finance/budgets?month=&year=` | Budget + realisasi | ✅ |
| POST | `/api/finance/budgets` | Set budget | ✅ |
| GET | `/api/finance/saving-goals` | Daftar target tabungan | ✅ |
| GET | `/api/finance/categories` | Daftar kategori | ✅ |
| GET | `/api/finance/reports/monthly` | Laporan bulanan | ✅ |

### Workout
| Method | Endpoint | Deskripsi | Auth |
|--------|----------|-----------|------|
| GET | `/api/workout/activities` | Daftar aktivitas | ✅ |
| POST | `/api/workout/activities` | Tambah manual | ✅ |
| GET | `/api/workout/stats?month=&year=` | Statistik workout | ✅ |
| GET | `/api/workout/personal-records` | Personal record per tipe | ✅ |

### Strava
| Method | Endpoint | Deskripsi | Auth |
|--------|----------|-----------|------|
| GET | `/api/strava/auth-url` | Dapat URL OAuth Strava | ✅ |
| POST | `/api/strava/exchange-token` | Tukar code → token | ✅ |
| POST | `/api/strava/sync` | Sinkron aktivitas terbaru | ✅ |
| DELETE | `/api/strava/disconnect` | Putus koneksi Strava | ✅ |

---

## 12. Docker Setup

### docker-compose.yml

```yaml
version: '3.9'

services:
  postgres:
    image: postgres:16-alpine
    container_name: fitrack_db
    environment:
      POSTGRES_USER: fitrack_user
      POSTGRES_PASSWORD: fitrack_pass_2024
      POSTGRES_DB: fitrack_db
    ports:
      - "5432:5432"
    volumes:
      - fitrack_pgdata:/var/lib/postgresql/data
      - ./docker/postgres/init.sql:/docker-entrypoint-initdb.d/init.sql
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U fitrack_user -d fitrack_db"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    container_name: fitrack_redis
    ports:
      - "6379:6379"
    volumes:
      - fitrack_redis_data:/data
    command: redis-server --appendonly yes
    restart: unless-stopped

volumes:
  fitrack_pgdata:
  fitrack_redis_data:
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitrack_db;Username=fitrack_user;Password=fitrack_pass_2024"
  },
  "Jwt": {
    "Secret": "GANTI_DENGAN_SECRET_MINIMAL_32_KARAKTER_RANDOM",
    "Issuer": "FiTrack",
    "Audience": "FiTrack-Users"
  },
  "Strava": {
    "ClientId": "YOUR_STRAVA_CLIENT_ID",
    "ClientSecret": "YOUR_STRAVA_CLIENT_SECRET"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Perintah Docker

```bash
# Jalankan database
docker compose up -d

# Cek status
docker compose ps

# Lihat log PostgreSQL
docker compose logs postgres -f

# Jalankan migrasi EF Core
cd backend
dotnet ef migrations add InitialCreate --project src/FiTrack.Infrastructure --startup-project src/FiTrack.API
dotnet ef database update --project src/FiTrack.Infrastructure --startup-project src/FiTrack.API

# Jalankan backend
cd src/FiTrack.API
dotnet run

# Jalankan frontend (terminal terpisah)
cd frontend/fitrack-web
ng serve
```

---

## 13. Roadmap Fitur

### Phase 1 — MVP (4-6 minggu)
- [x] Setup project structure & Docker
- [x] Autentikasi JWT + Session Manager
- [x] CRUD Transaksi & Kategori
- [ ] Dashboard ringkasan keuangan
- [ ] Koneksi Strava OAuth
- [ ] Sync & tampil aktivitas Strava
- [ ] UI dasar dengan sidebar + NG-Zorro

### Phase 2 — Core Features (4 minggu)
- [ ] Budget per kategori dengan visualisasi
- [ ] Target tabungan & progress tracking
- [ ] Grafik pengeluaran (Chart.js)
- [ ] Personal records workout
- [ ] Kalender aktivitas (heatmap)
- [ ] Recurring bills reminder

### Phase 3 — Advanced (4 minggu)
- [ ] Analisis korelasi keuangan vs aktivitas
- [ ] Export laporan PDF & Excel
- [ ] Multi-rekening & transfer antar rekening
- [ ] Notifikasi sistem (tagihan jatuh tempo, reminder latihan)
- [ ] Dark mode
- [ ] Backup & restore data lokal

### Phase 4 — Polish
- [ ] PWA / Electron wrapper untuk desktop native
- [ ] Onboarding flow untuk pengguna baru
- [ ] Import transaksi dari CSV/Excel
- [ ] Widget ringkasan "Hari Ini" di beranda

---

## 📚 Referensi

| Sumber | URL |
|--------|-----|
| .NET 10 Docs | https://learn.microsoft.com/dotnet |
| Angular 18 | https://angular.dev |
| NG-Zorro | https://ng.ant.design |
| Tailwind CSS | https://tailwindcss.com |
| Strava API | https://developers.strava.com |
| EF Core + PostgreSQL | https://www.npgsql.org/efcore |
| Clean Architecture | https://github.com/ardalis/CleanArchitecture |
| MediatR | https://github.com/jbogard/MediatR |

---

> **Catatan:** File ini adalah panduan living document. Update seiring progress development.
> Dibuat untuk: FiTrack v1.0 · .NET 10 + Angular 18
