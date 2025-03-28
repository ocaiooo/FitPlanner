using Microsoft.Extensions.Configuration;

namespace FitPlanner.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("Connection");
}