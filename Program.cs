using Backengv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;
using Backengv2.Profiles;
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
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

