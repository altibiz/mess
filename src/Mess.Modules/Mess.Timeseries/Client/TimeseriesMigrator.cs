using System.Reflection;
using Mess.Timeseries.Extensions.Microsoft;
using Mess.Timeseries.Abstractions.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Mess.Tenants;

namespace Mess.Timeseries.Client;

public class TimeseriesMigrator : ITimeseriesMigrator
{
  public async Task MigrateAsync()
  {
    foreach (var tenant in _tenants.All)
    {
      await _tenants.ImpersonateAsync(
        tenant,
        async () =>
        {
          foreach (var context in _services.GetServicesInheriting<DbContext>())
          {
            await context.Database.MigrateAsync();
            _logger.LogDebug(
              "Migrated database {}",
              context.Database.GetConnectionString()
            );

            await context.Database.ExecuteSqlRawAsync(
              "CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;"
            );

            var entityTypes = context.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
              foreach (var property in entityType.GetProperties())
              {
                if (
                  property.PropertyInfo?.GetCustomAttribute(
                    typeof(HypertableColumnAttribute)
                  ) != null
                )
                {
                  var tableName = entityType.GetTableName();
                  // TODO: use non deprecated version
#pragma warning disable CS0618
                  var columnName = property.GetColumnName();
#pragma warning restore CS0618
                  try
                  {
                    await context.Database.ExecuteSqlRawAsync(
                      $"SELECT create_hypertable('\"{tableName}\"', '{columnName}');"
                    );
                  }
                  catch (PostgresException exception)
                  {
                    // NOTE: hypertable already exists
                    if (exception.SqlState != "TS110")
                    {
                      throw exception;
                    }
                  }

                  _logger.LogDebug(
                    "Created hypertable {} for column {}",
                    tableName,
                    columnName
                  );
                }
              }
            }
          }

          _logger.LogInformation("Timeseries migrated");
        }
      );
    }
  }

  public void Migrate()
  {
    foreach (var tenant in _tenants.All)
    {
      _tenants.Impersonate(
        tenant,
        () =>
        {
          foreach (var context in _services.GetServicesInheriting<DbContext>())
          {
            context.Database.Migrate();
            _logger.LogDebug(
              "Migrated database {}",
              context.Database.GetConnectionString()
            );

            context.Database.ExecuteSqlRaw(
              "CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;"
            );
            var entityTypes = context.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
              foreach (var property in entityType.GetProperties())
              {
                if (
                  property.PropertyInfo?.GetCustomAttribute(
                    typeof(HypertableColumnAttribute)
                  ) != null
                )
                {
                  var tableName = entityType.GetTableName();
                  // TODO: use non deprecated version
#pragma warning disable CS0618
                  var columnName = property.GetColumnName();
#pragma warning restore CS0618
                  try
                  {
                    context.Database.ExecuteSqlRaw(
                      $"SELECT create_hypertable('\"{tableName}\"', '{columnName}');"
                    );
                  }
                  catch (PostgresException exception)
                  {
                    // NOTE: hypertable already exists
                    if (exception.SqlState != "TS110")
                    {
                      throw exception;
                    }
                  }

                  _logger.LogDebug(
                    "Created hypertable {} for column {}",
                    tableName,
                    columnName
                  );
                }
              }
            }
          }
          _logger.LogInformation("Timeseries migrated");
        }
      );
    }
  }

  public TimeseriesMigrator(
    IServiceProvider services,
    ILogger<TimeseriesMigrator> logger,
    ITenants tenants
  )
  {
    _services = services;
    _logger = logger;
    _tenants = tenants;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
  private readonly ITenants _tenants;
}
