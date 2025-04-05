using System.Reflection;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories;
using FitPlanner.Domain.Respositories.User;
using FitPlanner.Infrastructure.DataAccess;
using FitPlanner.Infrastructure.DataAccess.Repositories;
using FitPlanner.Infrastructure.Extensions;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        
        if (configuration.IsUnitTestEnviroment())
            return;
        
        AddDbContext(services, configuration);
        AddFluentMigrator(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FitPlannerDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(configuration.GetConnectionString("Connection"));
        });
    }
    
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("FitPlanner.Infrastructure")).For.All();
        });
    } 
}