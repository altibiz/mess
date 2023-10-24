using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using YesSql;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.ViewModels;

namespace Mess.Billing.Drivers;

public class InvoicePartDisplayDriver : ContentPartDisplayDriver<InvoicePart>
{
  public override IDisplayResult Display(
    InvoicePart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<InvoicePartViewModel>(
        GetDisplayShapeType(context),
        model =>
        {
          model.Part = part;
          model.Definition = context.TypePartDefinition;
          model.ContentItemId = part.ContentItem.ContentItemId;
        }
      )
      .Location("Detail", "Content");
  }

  public InvoicePartDisplayDriver(
    IServiceProvider serviceProvider,
    IStringLocalizer<InvoicePartDisplayDriver> localizer,
    ISession session
  )
  {
    _serviceProvider = serviceProvider;
    _session = session;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IServiceProvider _serviceProvider;
  private readonly ISession _session;
}
