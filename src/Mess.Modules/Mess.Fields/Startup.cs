using Fluid;
using Mess.Fields.Abstractions.Fields;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Drivers;
using Mess.Iot.Security;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.Fields;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();
    services.AddNavigationProvider<AdminMenu>();

    // ApiKeyField
    services.Configure<TemplateOptions>(options =>
    {
      options.MemberAccessStrategy.Register<ApiKeyField>();
    });
    services
      .AddContentField<ApiKeyField>()
      .UseDisplayDriver<ApiKeyFieldDisplayDriver>();
    services.AddContentPartFieldDefinitionDisplayDriver<ApiKeyFieldSettingsDriver>();
    services.AddSingleton<IApiKeyFieldService, ApiKeyFieldService>();

    // IntervalField
    services.Configure<TemplateOptions>(options =>
    {
      options.MemberAccessStrategy.Register<IntervalField>();
    });
    services
      .AddContentField<IntervalField>()
      .UseDisplayDriver<IntervalFieldDisplayDriver>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  ) { }
}
