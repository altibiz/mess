using Mess.EventStore.Client;
using Mess.Util.OrchardCore.Tenants;
using Mess.EventStore.Test.Extensions.Npgsql;
using Npgsql;

namespace Mess.EventStore.Test;

// TODO: use schemas for isolation https://stackoverflow.com/questions/39439879/npgsql-with-netcore-how-to-login-to-schema-not-just-to-database

public record class DatabaseIsolatedTest
{
  public DatabaseIsolatedTest(IEventStoreClient client, ITenantProvider tenant)
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

  protected IEventStoreClient Client { get; }
}
