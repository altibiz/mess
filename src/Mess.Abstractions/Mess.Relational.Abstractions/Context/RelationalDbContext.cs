using System.Linq.Expressions;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Relational.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchardCore.Environment.Shell;

namespace Mess.Relational.Abstractions.Context;

public abstract class RelationalDbContext : DbContext
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(TenantedEntity),
      entity =>
        entity.HasKey(nameof(TenantedEntity.Tenant), nameof(TenantedEntity.Id))
    );
  }

  protected void CreateTenantedEntitiesWithBase(
    ModelBuilder builder,
    Type @base,
    Action<EntityTypeBuilder>? configuration = null
  )
  {
    foreach (
      var entityType in GetType().Assembly
        .GetTypes()
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
              Expression.Constant(DatabaseTablePrefix)
            ),
            new List<ParameterExpression>() { entityParameter }
          )
        );
      configuration?.Invoke(entity);
    }
  }

  protected string DatabaseTablePrefix =>
    _shellSettings.GetDatabaseTablePrefix();

  protected string DatabaseConnectionString =>
    _shellSettings.GetDatabaseConnectionString();

  public RelationalDbContext(
    DbContextOptions options,
    ShellSettings shellSettings
  )
    : base(options)
  {
    _shellSettings = shellSettings;
  }

  private readonly ShellSettings _shellSettings;
}
