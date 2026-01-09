using FluentMigrator;

namespace FitPlanner.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TableRefreshToken, "Create table to save refresh tokens")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("RefreshTokens")
            .WithColumn("Token").AsString(100).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_Users", "Users", "Id")
            .WithColumn("ExpiresOn").AsDateTime2().NotNullable();
    }
}