using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using Mess.Iot.Abstractions.Extensions;
using Mess.Enms.Iot;

namespace Mess.Enms;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    // Migrations
    services.AddDataMigration<Migrations>();

    // Resources
    services.AddResources<Resources>();

    // Populations
    // services.AddPopulation<Populations>();

    // Iot
    services.AddIotPushHandler<EgaugePushHandler>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  ) { }
}
