using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using DataAccess.Data;
using DataAccess.Repositories;
using IBusinessLogic;
using IDataAccess;
using LogicInterface;
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

        services.AddScoped<ISessionLogic, SessionLogic>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserLogic, UserLogic>();
        
    }
}
