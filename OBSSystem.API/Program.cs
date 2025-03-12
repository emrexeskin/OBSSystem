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
using OBSSystem.Core.Configuration;
using OBSSystem.Infrastructure.Configurations.OBSSystem.Infrastructure.Configurations;


var builder = WebApplication.CreateBuilder(args);
var corsPolicy = "_obsCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Frontend URL
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
    
    
});

//IOptions Kullanarak JWT Konfigürasyonunu Bağla
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

// **Veritabanı Bağlantısı**
builder.Services.AddDbContext<OBSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **Service Kayıtları (Scoped Bağımlılıklar)**
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<ActivityLogService>();
// **JWT Authentication Yapılandırması**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>();
        if (jwtConfig == null)
        {
            throw new Exception("JWT yapılandırması bulunamadı. Lütfen 'appsettings.json' dosyasını kontrol edin.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
        };
    });

// **Yetkilendirme**
builder.Services.AddAuthorization();

// **Controller ve JSON Ayarları**
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });



// **OpenAPI (Swagger) Entegrasyonu**
builder.Services.AddEndpointsApiExplorer();

// **Loglama**
builder.Services.AddLogging();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OBSContext>();
    context.Database.Migrate();
    DbInitializer.Seed(context);
}

app.UseMiddleware<ExceptionMiddleware>(); // Hata yönetimi middleware
app.UseAuthentication(); // Kimlik doğrulama
app.UseMiddleware<TokenBlacklistMiddleware>(); // Blacklist middleware
app.UseAuthorization(); // Yetkilendirme middleware
app.MapControllers(); // API rotalarını bağlama
app.UseCors(corsPolicy);

// **Uygulamayı Başlat**
app.Run();
