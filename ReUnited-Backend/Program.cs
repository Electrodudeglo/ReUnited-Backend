using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ReUnited_Backend;
using ReUnited_Backend.DbContexts;
using ReUnited_Backend.Middleware;
using ReUnited_Backend.Repositories;
using ReUnited_Backend.Services;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<ILostItemService, LostItemService>();
builder.Services.AddScoped<ILostItemRepository, LostItemRepository>();




// Add services to the container.

builder.Services.AddDbContext<LostItemDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<LostItemDbContext>();

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

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

app.Run();
