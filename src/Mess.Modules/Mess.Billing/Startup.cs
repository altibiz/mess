using Mess.Billing.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.Billing;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

    services.AddContentPart<LegalEntityPart>();

    services.AddContentPart<ReceiptPart>();

    services.AddContentPart<InvoicePart>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  ) { }
}
