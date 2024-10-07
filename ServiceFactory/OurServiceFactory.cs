using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using DataAccess;
using DataAccess.Data;
using DataAccess.Repositories;
using IBusinessLogic;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceFactory;

[ExcludeFromCodeCoverage]
public static class OurServiceFactory
{
    public static void AddServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<HMDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHomeRepository, HomeRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IMemberSettingRepository, MemberSettingRepository>();
        services.AddScoped<IMemberSettingLogic, MemberSettingLogic>();
        services.AddScoped<IHomeLogic, HomeLogic>();
        services.AddScoped<IUserLogic, UserLogic>();
        services.AddScoped<ICompanyLogic, CompanyLogic>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IDeviceLogic, DeviceLogic>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
    }
}
