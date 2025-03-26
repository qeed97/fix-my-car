using System.Text;
using BackendServer.Data;
using BackendServer.Models.UserModels;
using BackendServer.Services.AuthenticationServices.AuthenticationSeeder;
using BackendServer.Services.AuthenticationServices.TokenService;
using BackendServer.Services.FixServices.Factory;
using BackendServer.Services.FixServices.Repository;
using BackendServer.Services.ProblemServices.Factory;
using BackendServer.Services.ProblemServices.Repository;
using BackendServer.Services.UserServices.Factory;
using BackendServer.Services.UserServices.Repository;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load("../../.env");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            []
        }
    });
});
builder.Services.AddDbContext<ApiDbContext>(options => { options.UseMySql(GetConnString(), new MySqlServerVersion(new Version(8, 0, 30))); });

builder.Services.AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>();

builder.Services.AddSingleton<IUserFactory, UserFactory>();
builder.Services.AddSingleton<IProblemFactory, ProblemFactory>();
builder.Services.AddSingleton<IFixFactory, FixFactory>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
builder.Services.AddScoped<IFixRepository, FixRepository>();
builder.Services.AddScoped<AuthenticationSeeder>();

AddJwt();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var context = services.GetRequiredService<ApiDbContext>();
if (context.Database.IsRelational()) context.Database.Migrate();
var authenticationSeeder = services.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.SeedRoles();

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

app.UseAuthentication();
    
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

void AddJwt()
{
    var issuingKey = "";
    if (builder.Environment.IsEnvironment("Testing"))
    {
        issuingKey = "dfhuiaozt478356784ztwuerz7ö3qwtzrugfhsoi";
    }
    else
    {
        issuingKey = Environment.GetEnvironmentVariable("ISSUING_KEY") ??
                     throw new Exception("ISSUING_KEY not found");
    }
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "fixmy",
                ValidAudience = "fixmy",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuingKey)
                )
            };
        });
}

app.Run();

public partial class Program { }