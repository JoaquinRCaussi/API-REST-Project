using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using DataAccess.Data;
using DataAccess.Repositories;
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
            options.UseSqlite(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserLogic, UserLogic>();
    }
}
