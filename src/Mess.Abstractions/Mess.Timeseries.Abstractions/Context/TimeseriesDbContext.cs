using Microsoft.EntityFrameworkCore;
using Mess.Relational.Abstractions.Context;
using OrchardCore.Environment.Shell;
using Mess.Timeseries.Abstractions.Entities;
using System.Reflection;

namespace Mess.Timeseries.Abstractions.Context;

public abstract class TimeseriesDbContext : RelationalDbContext
{
  protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder
  ) => optionsBuilder.UseNpgsql(DatabaseConnectionString);

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresExtension("timescaledb");

    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(HypertableEntity),
      entity =>
      {
        entity.HasKey(
          nameof(HypertableEntity.Tenant),
          nameof(HypertableEntity.Source),
          nameof(HypertableEntity.Timestamp)
        );

        entity.HasIndex(
          nameof(HypertableEntity.Tenant),
          nameof(HypertableEntity.Source),
          nameof(HypertableEntity.Timestamp)
        );

        entity.HasIndex(
          nameof(HypertableEntity.Tenant),
          nameof(HypertableEntity.Source),
          nameof(HypertableEntity.Milliseconds)
        );
      }
    );

    CreateEnumPropertyTypes(modelBuilder, typeof(HypertableEntity));
  }

  protected void CreateEnumPropertyTypes(ModelBuilder modelBuilder, Type @base)
  {
    foreach (
      var entityType in GetType().Assembly
        .GetTypes()
        .Where(
          type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(@base)
        )
    )
    {
      foreach (var property in entityType.GetProperties())
      {
        if (property.PropertyType.IsEnum)
        {
          var enumType = property.PropertyType;
          var genericMethod = HasPostgresEnumMethod.MakeGenericMethod(enumType);
          genericMethod.Invoke(
            null,
            new object?[] { modelBuilder, null, null, null }
          );
        }
      }
    }
  }

  private static readonly MethodInfo HasPostgresEnumMethod =
    typeof(NpgsqlModelBuilderExtensions)
      .GetMethods()
      .First(
        method =>
          method.IsGenericMethod
          && method.Name == nameof(NpgsqlModelBuilderExtensions.HasPostgresEnum)
      )
    ?? throw new InvalidOperationException("HasPostgresEnum method not found");

  public TimeseriesDbContext(
    DbContextOptions options,
    ShellSettings shellSettings
  )
    : base(options, shellSettings) { }
}
