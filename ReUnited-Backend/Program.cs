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

builder.Services.AddScoped<IFoundItemService, FoundItemService>();
builder.Services.AddScoped<IFoundItemRepository, FoundItemRepository>();

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

builder.Services.AddDbContext<FoundItemDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("SupabaseDb")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<FoundItemDbContext>();


var supabaseURL = builder.Configuration["Supabase:URL"];
var supabaseAnonKey = builder.Configuration["Supabase:Key"];
var supabaseIssuer = builder.Configuration["Supabase:Issuer"];
var supabaseAudience = builder.Configuration["Supabase:Audience"];



var sOptions = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabaseClient = new Supabase.Client(supabaseURL, supabaseAnonKey, sOptions);
await supabaseClient.InitializeAsync();
builder.Services.AddSingleton(supabaseClient);


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
                var jwksUrl = $"{supabaseIssuer}/.well-known/jwks.json";

                using var client = new HttpClient();
                var jwksJson = client.GetStringAsync(jwksUrl).Result;
                var jwks = new JsonWebKeySet(jwksJson);

                return jwks.GetSigningKeys();
            }
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FoundItemDbContext>();
    db.Database.Migrate();
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