using Microsoft.EntityFrameworkCore;
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
      builder.Entity(entityType).HasNoKey();
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
