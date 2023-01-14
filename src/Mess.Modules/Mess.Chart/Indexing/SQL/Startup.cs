using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Modules;

namespace Mess.Chart.Indexing.SQL;

[RequireFeatures("OrchardCore.ContentFields.Indexing.SQL")]
public class IndexingStartup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IScopedIndexProvider, ChartFieldIndexProvider>();
  }
}
