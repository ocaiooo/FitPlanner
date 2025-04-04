using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace FitPlanner.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    public ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime2().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable();
    }
}