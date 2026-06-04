using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReUnited_Backend;
using ReUnited_Backend.Controllers;
using ReUnited_Backend.DbContexts;
using ReUnited_Backend.Middleware;
using ReUnited_Backend.Repositories;
using ReUnited_Backend.Services;
using Supabase;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<ILostItemService, LostItemService>();
builder.Services.AddScoped<ILostItemRepository, LostItemRepository>();

builder.Services.AddHttpClient();

builder.Services
    .AddOptions<SupabaseSettings>()
    .Bind(builder.Configuration.GetSection("Supabase"))
    .Validate(settings =>
        !string.IsNullOrWhiteSpace(settings.Url),
        "Supabase Url is required")
    .Validate(settings =>
        !string.IsNullOrWhiteSpace(settings.Bucket),
        "Supabase Bucket is required")
    .Validate(settings =>
        !string.IsNullOrWhiteSpace(settings.ApiKey),
        "Supabase ApiKey is required")
    .ValidateOnStart();

builder.Services.AddScoped<IImageStorageService, ImageStorageService>();
builder.Services.AddScoped<ImageUrlService>();

// Add services to the container.

builder.Services.AddDbContext<LostItemDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("SupabaseDb")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<LostItemDbContext>();

//var key = Encoding.UTF8.GetBytes("a-string-secret-at-least-256-bits-long");

// JWT building:

/*{
    "sub": "1234567890",
  "name": "John",
  "iss": "ReUnite",
  "aud": "ReUnite",
  "iat": 1780304665,
  "exp": 1811840665,
  "roles": "admin"
}*/

// JWT
// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4iLCJpc3MiOiJSZVVuaXRlIiwiYXVkIjoiUmVVbml0ZSIsImlhdCI6MTc4MDMwNDY2NSwiZXhwIjoxODExODQwNjY1LCJyb2xlcyI6ImFkbWluIn0.mqL1yD0G7cGhZ6uReIzO49lg-tlBaDD4vz-PWg1CSHk

//var jwtKey = builder.Configuration["Supabase:JWTKey"];

var supabaseURL = builder.Configuration["Supabase:URL"];
// The Anon key is strictly for the Supabase Client (PostgREST/Realtime)
var supabaseAnonKey = builder.Configuration["Supabase:Key"];
// For ES256, ValidIssuer is typically your project URL or URL + "/auth/v1"
var supabaseIssuer = builder.Configuration["Supabase:Issuer"];
var supabaseAudience = builder.Configuration["Supabase:Audience"];



var sOptions = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabaseClient = new Supabase.Client(supabaseURL, supabaseAnonKey, sOptions);
await supabaseClient.InitializeAsync();
builder.Services.AddSingleton(supabaseClient);

//var supabaseSignatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseAnonKey));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = supabaseIssuer;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = supabaseIssuer,
            ValidateAudience = true,
            ValidAudience = supabaseAudience,
            ValidateLifetime = true,

            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                // Construct the exact URL to Supabase's JWKS endpoint
                var jwksUrl = $"{supabaseIssuer}/.well-known/jwks.json";

                // Note: In production, you should cache this HttpClient request
                using var client = new HttpClient();
                var jwksJson = client.GetStringAsync(jwksUrl).Result;
                var jwks = new JsonWebKeySet(jwksJson);

                return jwks.GetSigningKeys();
            }
        };
    });



// temp, delete later
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
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

Console.WriteLine($"[DEBUG] Issuer: '{builder.Configuration["Supabase:Issuer"]}'");
Console.WriteLine($"[DEBUG] Audience: '{builder.Configuration["Supabase:Audience"]}'");

//builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LostItemDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

app.Run();