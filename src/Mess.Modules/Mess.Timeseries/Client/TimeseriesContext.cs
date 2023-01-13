using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mess.Tenants;
using Mess.Timeseries.Entities;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mess.Timeseries.Client;

// TODO: inteface somehow
public class TimeseriesContext : DbContext
{
  public DbSet<Measurement> Measurements { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
    builder.UseNpgsql(Tenants.GetTenantConnectionString());

  protected override void OnModelCreating(ModelBuilder builder)
  {
    CreateEntitiesWithBase(builder, typeof(TenantEntity));
    CreateEntitiesWithBase(
      builder,
      typeof(HypertableEntity),
      entity => entity.HasKey("Timestamp", "Id")
    );
    CreateEntitiesWithBase(
      builder,
      typeof(MeasurementEntity),
      entity => entity.HasKey("Timestamp", "SourceId")
    );
  }

  private void CreateEntitiesWithBase(
    ModelBuilder builder,
    Type @base,
    Action<EntityTypeBuilder>? configuration = null
  )
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
            && type.IsAssignableTo(@base)
        )
    )
    {
      var entityParameter = Expression.Parameter(entityType);
      var entity = builder
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
      configuration?.Invoke(entity);
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

public static class TimeseriesContextServiceProviderExtensions
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
