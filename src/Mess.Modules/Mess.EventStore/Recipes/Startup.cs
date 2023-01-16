using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Recipes;

namespace Mess.EventStore.Recipes;

[RequireFeatures("OrchardCore.Recipes")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddRecipeExecutionStep<EventImportStep>();
  }
}
