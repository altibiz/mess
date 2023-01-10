using System.Reflection;
using Mess.Timeseries.Extensions.Microsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Mess.Timeseries.Client;

[AttributeUsage(AttributeTargets.Property)]
public class HypertableColumnAttribute : Attribute { }

public class TimeseriesMigrator : ITimeseriesMigrator
{
  public async Task MigrateAsync()
  {
    foreach (var context in Contexts)
    {
      await context.Database.MigrateAsync();
      Logger.LogDebug(
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

            Logger.LogDebug(
              "Created hypertable {} for column {}",
              tableName,
              columnName
            );
          }
        }
      }
    }

    Logger.LogInformation("Timeseries migrated");
  }

  public void Migrate()
  {
    foreach (var context in Contexts)
    {
      context.Database.Migrate();
      Logger.LogDebug(
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

            Logger.LogDebug(
              "Created hypertable {} for column {}",
              tableName,
              columnName
            );
          }
        }
      }
    }
    Logger.LogInformation("Timeseries migrated");
  }

  public TimeseriesMigrator(
    IServiceProvider services,
    ILogger<TimeseriesMigrator> logger
  )
  {
    Contexts = services.GetServicesInheriting<DbContext>().ToList();
    Logger = logger;
  }

  IEnumerable<DbContext> Contexts { get; }
  ILogger Logger { get; }
}
