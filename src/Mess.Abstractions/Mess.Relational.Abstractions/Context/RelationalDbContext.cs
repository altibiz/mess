using System.Linq.Expressions;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Relational.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchardCore.Environment.Shell;

namespace Mess.Relational.Abstractions.Context;

public abstract class RelationalDbContext : DbContext
{
  private readonly ShellSettings _shellSettings;

  private readonly IHostEnvironment _environment;

  public RelationalDbContext(
    DbContextOptions options,
    IServiceProvider services
  )
    : base(options)
  {
    _shellSettings = services.GetRequiredService<ShellSettings>();
    _environment = services.GetRequiredService<IHostEnvironment>();
  }

  protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder
  )
  {
    if (_environment.IsDevelopment())
    {
      // optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
      // optionsBuilder.EnableSensitiveDataLogging();
      // optionsBuilder.EnableDetailedErrors();
    }
  }

  protected string DatabaseTablePrefix =>
    _shellSettings.GetDatabaseTablePrefix();

  protected string DatabaseConnectionString =>
    _shellSettings.GetDatabaseConnectionString();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    CreateTenantedEntitiesWithBase(
      modelBuilder,
      typeof(TenantedEntity),
      (_, entity) =>
      {
        entity.HasKey(
          nameof(TenantedEntity.Tenant),
          nameof(TenantedEntity.Id)
        );
      }
    );
  }

  protected void CreateTenantedEntitiesWithBase(
    ModelBuilder builder,
    Type @base,
    Action<Type, EntityTypeBuilder>? configuration = null
  )
  {
    foreach (
      var entityType in GetType().Assembly
        .GetTypes()
        .Where(
          type => type.IsClass
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
              Expression.Constant(DatabaseTablePrefix)
            ),
            new List<ParameterExpression> { entityParameter }
          )
        );
      configuration?.Invoke(entityType, entity);
    }
  }
}
