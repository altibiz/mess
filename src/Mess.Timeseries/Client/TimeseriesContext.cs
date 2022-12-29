using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mess.Util.OrchardCore.Tenants;
using Mess.Timeseries.Entities;
using System.Reflection;
using System.Linq.Expressions;

namespace Mess.Timeseries.Client;

public class TimeseriesContext : DbContext
{
  public DbSet<Measurement> Measurements { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
    builder.UseNpgsql(Tenants.GetTenantConnectionString());

  protected override void OnModelCreating(ModelBuilder builder)
  {
    foreach (
      var entityType in Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(
          type =>
            type.Namespace == "Mess.Timeseries.Entities"
            && type.IsClass
            && !type.IsAbstract
            && type.IsAssignableTo(typeof(TenantEntity))
        )
    )
    {
      var entityParameter = Expression.Parameter(entityType);
      builder
        .Entity(entityType)
        .HasQueryFilter(
          Expression.Lambda(
            Expression.Equal(
              Expression.MakeMemberAccess(
                entityParameter,
                entityType.GetMember("Tenant").First()
              ),
              Expression.Constant(Tenants.GetTenantName())
            ),
            new List<ParameterExpression>() { entityParameter }
          )
        );
    }

    foreach (
      var entityType in Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(
          type =>
            type.Namespace == "Mess.Timeseries.Entities"
            && type.IsClass
            && !type.IsAbstract
            && type.IsAssignableTo(typeof(HypertableEntity))
        )
    )
    {
      var entityParameter = Expression.Parameter(entityType);
      builder
        .Entity(entityType)
        .HasQueryFilter(
          Expression.Lambda(
            Expression.Equal(
              Expression.MakeMemberAccess(
                entityParameter,
                entityType.GetMember("Tenant").First()
              ),
              Expression.Constant(Tenants.GetTenantName())
            ),
            new List<ParameterExpression>() { entityParameter }
          )
        )
        .HasKey("Timestamp", "Id");
    }

    foreach (
      var entityType in Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(
          type =>
            type.Namespace == "Mess.Timeseries.Entities"
            && type.IsClass
            && !type.IsAbstract
            && type.IsAssignableTo(typeof(MeasurementEntity))
        )
    )
    {
      var entityParameter = Expression.Parameter(entityType);
      builder
        .Entity(entityType)
        .HasQueryFilter(
          Expression.Lambda(
            Expression.Equal(
              Expression.MakeMemberAccess(
                entityParameter,
                entityType.GetMember("Tenant").First()
              ),
              Expression.Constant(Tenants.GetTenantName())
            ),
            new List<ParameterExpression>() { entityParameter }
          )
        )
        .HasKey("Timestamp", "SourceId");
    }
  }

  public TimeseriesContext(
    DbContextOptions<TimeseriesContext> options,
    ITenantProvider tenants
  ) : base(options)
  {
    Tenants = tenants;
  }

  ITenantProvider Tenants { get; }
}

internal static class TimeseriesContextServiceProviderExtensions
{
  public static async Task<T> WithTimeseriesContextAsync<T>(
    this IServiceProvider services,
    Func<TimeseriesContext, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<TimeseriesContext>();
    var result = await todo(context);
    return result;
  }

  public static T WithTimeseriesContext<T>(
    this IServiceProvider services,
    Func<TimeseriesContext, T> todo
  )
  {
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TimeseriesContext>();
    var result = todo(context);
    return result;
  }
}
