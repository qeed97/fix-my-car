using BackendServer.Data;
using BackendServer.Services.ProblemServices.Factory;
using BackendServer.Services.ProblemServices.Repository;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend API",
        Version = "v1",
        Description = "API for managing car-related issues",
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApiDbContext>(options => { options.UseMySQL(GetConnString()); });

builder.Services.AddSingleton<IProblemFactory, ProblemFactory>();
builder.Services.AddScoped<IProblemRepository, ProblemRepository>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var context = services.GetRequiredService<ApiDbContext>();
if (context.Database.IsRelational()) context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at the root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

string GetConnString()
{
    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MySql");
    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        connectionString = $"Server={Environment.GetEnvironmentVariable("LOCAL_SERVER_NAME")};" +
                           $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                           $"User={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                           $"Password={Environment.GetEnvironmentVariable("DB_USER_PASSWORD")};" +
                           $"Port={Environment.GetEnvironmentVariable("DB_PORT")};";

    Console.WriteLine($"Connection string: {connectionString}");

    if (connectionString == null) throw new Exception("Could not find connection string");
    return connectionString;
}

app.Run();