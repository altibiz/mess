using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Mess.Timeseries.Client;

[AttributeUsage(AttributeTargets.Property)]
public class HypertableColumnAttribute : Attribute { }

public static class TimescaleExtensions
{
  public static void ApplyHypertables(this DbContext context)
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
        }
      }
    }
  }
}
