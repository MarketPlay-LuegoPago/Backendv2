using Backengv2.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;
=======
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Backengv2.Profiles;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Mvc;

using Backengv2.Services;


>>>>>>> 8897019b7930b8e922adcd7388608d7f50c8954f
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);



builder.Services.AddDbContext<BaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql")));
<<<<<<< HEAD

//Configuramos JsonWebToken
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(configure => {
        configure.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer  = "http://localhost:5090", //Here the endpoint
            ValidAudience =  "http://localhost:5090", //Here the other endpoint 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FTGUNMIMGI4MFI4J2RÑNUFRFFM4FN4874H4BBFHRF"))
        };
    });
//Agregamos los Services
builder.Services.AddScoped<IAuthRepository>(provider =>
new AuthRepository(provider.GetRequiredService<BaseContext>(), "FTGUNMIMGI4MFI4J2RÑNUFRFFM4FN4874H4BBFHRF"));
        
//builder.Services.AddAutoMapper(typeof(StudentP rofile), typeof(Teacher Profile), typeof(ClassProfile));

var app = builder.Build();
app.UseAuthentication();  //Agregamos permisos para DataConection
app.UseAuthorization();
=======



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
  

builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IMarketplaceUserRepository, MarketplaceUserRepository>();

builder.Services.AddScoped<MailerSendService>();


var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
>>>>>>> 8897019b7930b8e922adcd7388608d7f50c8954f
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

