using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.LogicInterfaces;
using DataAccess;
using DataAccess.Data;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApi.Filters;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HMDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IMemberSettingRepository, MemberSettingRepository>();
builder.Services.AddScoped<IMemberSettingLogic, MemberSettingLogic>();
builder.Services.AddScoped<IHomeLogic, HomeLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<ICompanyLogic, CompanyLogic>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IDeviceLogic, DeviceLogic>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

WebApplication? app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
