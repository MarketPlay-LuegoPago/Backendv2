using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;
using Backengv2.Profiles;
using Backengv2.Services;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Configuración inicial de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// Clave para la autenticación JWT
var key = Encoding.UTF8.GetBytes("ncjdncjvurbuedxwn233nnedxee+dfr-");

// Configuración de la autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:5205",
        ValidAudience = "https://localhost:5205",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Configuración de la autorización
builder.Services.AddAuthorization();

// Configuración para habilitar la exploración de API Endpoints
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen();

// Configuración de controladores MVC
builder.Services.AddControllers();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// Configuración del contexto de base de datos
builder.Services.AddDbContext<BaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql")));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Registro de dependencias de servicios
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IMarketplaceUserRepository, MarketplaceUserRepository>();
builder.Services.AddScoped<MailerSendService>();

// Construcción de la aplicación
var app = builder.Build();

// Uso de CORS
app.UseCors("AllowAnyOrigin");

// Configuración de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Uso de Swagger y Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Configuración adicional de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();

// Configuración adicional del pipeline de solicitudes HTTP según el entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección HTTPS y mapeo final de controladores
app.UseHttpsRedirection();
app.MapControllers();

// Ejecución de la aplicación
app.Run();
