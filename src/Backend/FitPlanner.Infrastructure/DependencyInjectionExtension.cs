using System.Reflection;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories;
using FitPlanner.Domain.Respositories.User;
using FitPlanner.Domain.Security.Cryptography;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Domain.Services.LoggedUser;
using FitPlanner.Infrastructure.DataAccess;
using FitPlanner.Infrastructure.DataAccess.Repositories;
using FitPlanner.Infrastructure.Extensions;
using FitPlanner.Infrastructure.Security.Cryptography;
using FitPlanner.Infrastructure.Security.Tokens.Access.Generator;
using FitPlanner.Infrastructure.Security.Tokens.Access.Validator;
using FitPlanner.Infrastructure.Services.LoggedUser;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncripter(services, configuration);
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        
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
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    }
    
    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        
        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(signingKey!, expirationTimeMinutes));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey!));
    }
    
    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        
        services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(additionalKey!));
    }
}