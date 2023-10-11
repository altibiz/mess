using Mess.Billing.Abstractions.Models;
using Mess.Billing.BackgroundTasks;
using Mess.Billing.Controllers;
using Mess.Billing.Handler;
using Mess.Billing.Indexes;
using Mess.Billing.Drivers;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.Billing;

public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddResources<Resources>();

    services.AddContentPart<LegalEntityPart>();

    services
      .AddContentPart<BillingPart>()
      .UseDisplayDriver<BillingPartDisplayDriver>();
    services.AddIndexProvider<BillingIndexProvider>();
    services.AddBackgroundTask<BillingBackgroundTask>();

    services.AddContentPart<ReceiptPart>();
    services
      .AddContentPart<InvoicePart>()
      .UseDisplayDriver<InvoicePartDisplayDriver>();
    services.AddIndexProvider<PaymentIndexProvider>();
    services.AddContentHandler<PaymentHandler>();
  }

  public override void Configure(
    IApplicationBuilder app,
    IEndpointRouteBuilder routes,
    IServiceProvider serviceProvider
  )
  {
    var adminUrlPrefix = app.ApplicationServices
      .GetRequiredService<IOptions<AdminOptions>>()
      .Value.AdminUrlPrefix;

    routes.MapAreaControllerRoute(
      name: "Mess.Billing.AdminController.CreateInvoice",
      areaName: "Mess.Billing",
      pattern: adminUrlPrefix + "/CreateInvoice/{contentItemId}",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.CreateInvoice)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Billing.AdminController.ConfirmPayment",
      areaName: "Mess.Billing",
      pattern: adminUrlPrefix + "/ConfirmPayment/{contentItemId}",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.ConfirmPayment)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Billing.AdminController.ListPayments",
      areaName: "Mess.Billing",
      pattern: adminUrlPrefix + "/ListPayments",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.ListPayments)
      }
    );

    routes.MapAreaControllerRoute(
      name: "Mess.Billing.AdminController.ListOwnPayments",
      areaName: "Mess.Billing",
      pattern: adminUrlPrefix + "/ListOwnPayments",
      defaults: new
      {
        controller = typeof(AdminController).ControllerName(),
        action = nameof(AdminController.ListOwnPayments)
      }
    );
  }
}
