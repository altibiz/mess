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
          nameof(HypertableEntity.Tenant),
          nameof(HypertableEntity.Source),
          nameof(HypertableEntity.Timestamp)
        );

        entity.HasIndex(
          nameof(HypertableEntity.Tenant),
          nameof(HypertableEntity.Source),
          nameof(HypertableEntity.Timestamp)
        );

        entity.ToTable(@type.Name.RegexRemove("Entity$") + "s");
      }
    );

    CreateEnumPropertyTypes(
      modelBuilder,
      typeof(HypertableEntity)
    );

    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(HypertableViewEntity),
      (@type, entity) =>
      {
        entity.HasKey(
          nameof(HypertableViewEntity.Tenant),
          nameof(HypertableViewEntity.Source),
          nameof(HypertableViewEntity.Timestamp)
        );

        entity.HasIndex(
          nameof(HypertableViewEntity.Tenant),
          nameof(HypertableViewEntity.Source),
          nameof(HypertableViewEntity.Timestamp)
        );

        entity.ToView(@type.Name.RegexRemove("Entity$"));
      }
    );

    CreateEnumPropertyTypes(
      modelBuilder,
      typeof(HypertableViewEntity)
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
