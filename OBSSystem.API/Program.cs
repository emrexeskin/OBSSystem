using Microsoft.EntityFrameworkCore;
using OBSSystem.Infrastructure.Configurations;
using OBSSystem.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OBSSystem.Application.Interfaces;
using OBSSystem.Application.Services;
using OBSSystem.API.Middleware;
using OBSSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Authorization Configuration
builder.Services.AddAuthorization();

// Add DbContext
builder.Services.AddDbContext<OBSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register PasswordHasher
// Register PasswordHasher (Non-static class)
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>(); // BCrypt tabanlý þifreleme servisi
                                                                     

// Add Controllers with JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add OpenAPI (Swagger)
builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Apply Pending Migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OBSContext>();
    context.Database.Migrate(); // Ensure database is up-to-date
    DbInitializer.Seed(context); // Seed initial data
}

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Uncomment this if HTTPS is required
// app.UseHttpsRedirection(); 

// Authentication Middleware
app.UseAuthentication();

// Authorization Middleware
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
