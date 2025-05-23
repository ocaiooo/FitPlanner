﻿using FluentMigrator;

namespace FitPlanner.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TableUser, "Create table to save the user's information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
    }
}