using Mess.Chart.Abstractions.Models;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using YesSql;
using Mess.Iot.Abstractions.Indexes;
using Mess.Fields.Abstractions;
using Mess.Ozds.Abstractions.Client;

namespace Mess.Iot.Chart;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    IHostEnvironment hostEnvironment,
    IContentManager contentManager,
    ISession session
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _hostEnvironment = hostEnvironment;
    _contentManager = contentManager;
    _session = session;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly IContentManager _contentManager;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ISession _session;
}
