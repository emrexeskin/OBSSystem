using Microsoft.EntityFrameworkCore;
using OBSSystem.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Add Controllers with JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add OpenAPI (Swagger)
builder.Services.AddOpenApi();



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

// app.UseHttpsRedirection(); Uncomment this if HTTPS is required
app.UseAuthentication(); // Authentication Middleware
app.UseAuthorization();  // Authorization Middleware

// Map Controllers
app.MapControllers();

app.Run();
