using YesSql;
using YesSql.Sql;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Microsoft.EntityFrameworkCore;
using Mess.Timeseries.Client;

namespace Mess.Timeseries;

public partial class Migrations : DataMigration
{
  public async Task<int> Create()
  {
    var timeseries = await TimeseriesFactory.CreateDbContextAsync();
    await timeseries.Database.MigrateAsync();
    var created = await timeseries.Database.EnsureCreatedAsync();
    if (created)
    {
      timeseries.ApplyHypertables();
    }

    return 1;
  }

  public Migrations(
    IHostEnvironment env,
    ILogger<Migrations> logger,
    IRecipeMigrator recipe,
    IContentDefinitionManager content,
    ISession session,
    IDbContextFactory<TimeseriesContext> timeseriesFactory
  )
  {
    Env = env;
    Logger = logger;

    Session = session;

    Recipe = recipe;
    Content = content;

    TimeseriesFactory = timeseriesFactory;
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

  private IDbContextFactory<TimeseriesContext> TimeseriesFactory { get; }
}
