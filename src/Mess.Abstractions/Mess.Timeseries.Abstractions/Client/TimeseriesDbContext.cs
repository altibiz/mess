using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mess.Tenants;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Timeseries.Abstractions.Client;

public abstract class TimeseriesDbContext : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
    builder.UseNpgsql(_tenants.Current.ConnectionString);

  protected override void OnModelCreating(ModelBuilder builder)
  {
    CreateEntitiesWithBase(
      builder,
      typeof(HypertableEntity),
      entity => entity.HasKey("Tenant", "Source", "Timestamp")
    );
  }

  private void CreateEntitiesWithBase(
    ModelBuilder builder,
    Type @base,
    Action<EntityTypeBuilder>? configuration = null
  )
  {
    foreach (
      var entityType in this.GetType()
        .Assembly.GetTypes()
        .Where(
          type => type.IsClass && !type.IsAbstract && type.IsAssignableTo(@base)
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
              // TODO: this has to be invoked and not a constant
              Expression.Constant(_tenants.Current.Name)
            ),
            new List<ParameterExpression>() { entityParameter }
          )
        );
      configuration?.Invoke(entity);
    }
  }

  public TimeseriesDbContext(DbContextOptions options, ITenants tenants)
    : base(options)
  {
    _tenants = tenants;
  }

  private readonly ITenants _tenants;
}
