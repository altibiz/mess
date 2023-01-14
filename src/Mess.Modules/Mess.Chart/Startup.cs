using Fluid;
using Mess.Chart.Drivers;
using Mess.Chart.Fields;
using Mess.Chart.Handlers;
using Mess.Chart.Indexing;
using Mess.Chart.Models;
using Mess.Chart.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Indexing;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace Mess.Chart;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      Resources
    >();

    services.Configure<TemplateOptions>(o =>
    {
      o.MemberAccessStrategy.Register<ChartField>();
      o.MemberAccessStrategy.Register<DisplayChartFieldViewModel>();
    });

    services
      .AddContentField<ChartField>()
      .UseDisplayDriver<ChartFieldDisplayDriver>();
    services.AddScoped<
      IContentPartFieldDefinitionDisplayDriver,
      ChartFieldSettingsDriver
    >();
    services.AddScoped<IContentFieldIndexHandler, ChartFieldIndexHandler>();

    services
      .AddContentPart<ChartPart>()
      .UseDisplayDriver<ChartPartDisplayDriver>()
      .AddHandler<ChartPartHandler>();
    services.AddScoped<
      IContentTypePartDefinitionDisplayDriver,
      ChartPartSettingsDisplayDriver
    >();
    services.AddScoped<IContentPartIndexHandler, ChartPartIndexHandler>();
  }
}
