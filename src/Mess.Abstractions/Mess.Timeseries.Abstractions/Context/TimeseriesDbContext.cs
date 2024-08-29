using System.Reflection;
using Mess.Prelude.Extensions.Strings;
using Mess.Relational.Abstractions.Context;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Timeseries.Abstractions.Context;

public abstract class TimeseriesDbContext : RelationalDbContext
{
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
    IServiceProvider services
  )
    : base(options, services)
  {
  }

  protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder
  )
  {
    base.OnConfiguring(optionsBuilder);

    optionsBuilder.UseNpgsql(DatabaseConnectionString);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.HasPostgresExtension("timescaledb");

    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(HypertableEntity),
      (@type, entity) =>
      {
        entity.HasKey(
          nameof(ContinuousAggregateEntity.Timestamp),
          nameof(ContinuousAggregateEntity.Source),
          nameof(ContinuousAggregateEntity.Tenant)
        );

        entity.HasIndex(
          nameof(ContinuousAggregateEntity.Timestamp),
          nameof(ContinuousAggregateEntity.Source),
          nameof(ContinuousAggregateEntity.Tenant)
        );
      }
    );

    CreateEnumPropertyTypes(
      modelBuilder,
      typeof(HypertableEntity)
    );

    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(ContinuousAggregateEntity),
      (@type, entity) =>
      {
        entity.HasKey(
          nameof(ContinuousAggregateEntity.Timestamp),
          nameof(ContinuousAggregateEntity.Source),
          nameof(ContinuousAggregateEntity.Tenant)
        );

        entity.HasIndex(
          nameof(ContinuousAggregateEntity.Timestamp),
          nameof(ContinuousAggregateEntity.Source),
          nameof(ContinuousAggregateEntity.Tenant)
        );
      }
    );

    CreateEnumPropertyTypes(
      modelBuilder,
      typeof(ContinuousAggregateEntity)
    );
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
      foreach (var property in entityType.GetProperties())
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
