using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace FitPlanner.Infrastructure.Migrations;

public class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated_SqlServer(connectionString);
        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated_SqlServer(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        var dabaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", dabaseName);
        
        var records = dbConnection.Query("SELECT database_id FROM sys.databases WHERE name = @name", parameters);
        
        if (records.Any() == false)
            dbConnection.Execute($"CREATE DATABASE {dabaseName}");
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.ListMigrations();
        
        runner.MigrateUp(); 
    }
}