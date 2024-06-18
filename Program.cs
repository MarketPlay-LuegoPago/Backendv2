using Backengv2.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Backengv2.Profiles;
using Backengv2.Services.Coupons;
using Microsoft.AspNetCore.Mvc;

using Backengv2.Services;


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
//app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

