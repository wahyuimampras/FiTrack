using AspNetCoreRateLimit;
using FiTrack.API.Middleware;
using FiTrack.Application;
using FiTrack.Infrastructure;
using FiTrack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddInfrastructure(builder.Configuration);

// Application (MediatR & AutoMapper & FluentValidation)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyReference).Assembly));

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
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// CORS (untuk Angular dev)
builder.Services.AddCors(opts =>
    opts.AddPolicy("FiTrackPolicy", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
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
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Auto Migrate and Seed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    await DatabaseSeeder.SeedAsync(dbContext);
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("FiTrackPolicy");
app.UseAuthentication();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.UseIpRateLimiting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();