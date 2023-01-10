using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.Timeseries.Client;

[AttributeUsage(AttributeTargets.Property)]
public class HypertableColumnAttribute : Attribute { }

public class TimescaleMigrator : ITimeseriesMigrator
{
  public void Migrate()
  {
    foreach (var context in Contexts)
    {
      var created = context.Database.EnsureCreated();
      if (!created)
      {
        throw new InvalidOperationException("Database not created");
      }

      context.Database.Migrate();
      ApplyHypertables();
    }
    Logger.LogInformation("Timeseries migrated");
  }

  public async Task MigrateAsync()
  {
    foreach (var context in Contexts)
    {
      var created = await context.Database.EnsureCreatedAsync();
      if (!created)
      {
        throw new InvalidOperationException("Database not created");
      }

      await context.Database.MigrateAsync();
      await ApplyHypertablesAsync();
    }
    Logger.LogInformation("Timeseries migrated");
  }

  private async Task ApplyHypertablesAsync()
  {
    foreach (var context in Contexts)
    {
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
            await context.Database.ExecuteSqlRawAsync(
              $"SELECT create_hypertable('\"{tableName}\"', '{columnName}');"
            );

            Logger.LogDebug(
              "Created hypertable {} for column {}",
              tableName,
              columnName
            );
          }
        }
      }
    }
  }

  private void ApplyHypertables()
  {
    foreach (var context in Contexts)
    {
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
            context.Database.ExecuteSqlRaw(
              $"SELECT create_hypertable('\"{tableName}\"', '{columnName}');"
            );

            Logger.LogDebug(
              "Created hypertable {} for column {}",
              tableName,
              columnName
            );
          }
        }
      }
    }
  }

  public TimescaleMigrator(
    IEnumerable<DbContext> contexts,
    ILogger<TimescaleMigrator> logger
  )
  {
    Contexts = contexts;
    Logger = logger;
  }

  IEnumerable<DbContext> Contexts { get; }
  ILogger Logger { get; }
}
