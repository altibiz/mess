using YesSql;
using YesSql.Sql;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Client;
using Mess.Timeseries.Migrations.OrchardCore.M0;

namespace Mess.Timeseries.Migrations.OrchardCore;

public partial class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    await using var scope = Services.CreateAsyncScope();
    var timeseries =
      scope.ServiceProvider.GetRequiredService<TimeseriesContext>();
    await timeseries.Database.MigrateAsync();
    var created = await timeseries.Database.EnsureCreatedAsync();
    if (created)
    {
      timeseries.ApplyHypertables();
    }
    Logger.LogInformation("Timeseries migrated");

    Recipe.ExecuteTimeseriesMigration(this);

    return 1;
  }

  public Migrations(
    IHostEnvironment env,
    ILogger<Migrations> logger,
    IRecipeMigrator recipe,
    IContentDefinitionManager content,
    ISession session,
    IServiceProvider services
  )
  {
    Env = env;
    Logger = logger;

    Session = session;

    Recipe = recipe;
    Content = content;

    Services = services;
  }

  private IHostEnvironment Env { get; }
  private ILogger Logger { get; }

  private ISession Session { get; }
  private ISchemaBuilder Schema
  {
    get => SchemaBuilder;
  }

  private IRecipeMigrator Recipe { get; }
  private IContentDefinitionManager Content { get; }

  private IServiceProvider Services { get; }
}
