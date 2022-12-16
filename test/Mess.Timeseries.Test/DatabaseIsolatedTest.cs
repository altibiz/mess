using Mess.Timeseries.Client;
using Mess.Util.OrchardCore.Tenants;
using Mess.Timeseries.Test.Extensions.Npgsql;
using Npgsql;

namespace Mess.Timeseries.Test;

// TODO: use schemas for isolation https://stackoverflow.com/questions/39439879/npgsql-with-netcore-how-to-login-to-schema-not-just-to-database

public class DatabaseIsolatedTest
{
  public DatabaseIsolatedTest(ITimeseriesClient client, ITenantProvider tenant)
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

    Client = client;
  }

  protected ITimeseriesClient Client { get; }
}
