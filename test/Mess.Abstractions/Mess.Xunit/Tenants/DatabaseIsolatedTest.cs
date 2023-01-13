using Npgsql;
using Mess.Tenants;
using Mess.System.Extensions.String;
using Mess.Xunit.Extensions.Npgsql;

namespace Mess.Xunit.Tenants;

// TODO: use schemas for isolation https://stackoverflow.com/questions/39439879/npgsql-with-netcore-how-to-login-to-schema-not-just-to-database

public record class DatabaseIsolatedTest
{
  public DatabaseIsolatedTest(ITenantProvider tenant)
  {
    var tenantConnectionString = tenant.GetTenantConnectionString();
    var tenantDatabase = tenantConnectionString.RegexReplace(
      $".*{ConnectionStringExtensions.CONNECTION_STRING_DATABASE_REGEX}.*",
      @"$1"
    );
    var tenantInitializationConnectionString =
      tenantConnectionString.RegexRemove(
        ConnectionStringExtensions.CONNECTION_STRING_DATABASE_REGEX
      );

    using var tenantConnection = new NpgsqlConnection(
      tenantInitializationConnectionString
    );
    tenantConnection.Open();

    using var dropDatabaseCommand = new NpgsqlCommand(
      $"DROP DATABASE IF EXISTS \"{tenantDatabase}\"",
      tenantConnection
    );
    dropDatabaseCommand.ExecuteNonQuery();

    using var createDatabaseCommand = new NpgsqlCommand(
      $"CREATE DATABASE \"{tenantDatabase}\"",
      tenantConnection
    );
    createDatabaseCommand.ExecuteNonQuery();

    tenantConnection.Close();
  }
}
