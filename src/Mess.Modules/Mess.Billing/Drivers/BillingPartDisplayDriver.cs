using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using YesSql;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.ViewModels;

namespace Mess.Billing.Drivers;

public class BillingPartDisplayDriver : ContentPartDisplayDriver<BillingPart>
{
  public override IDisplayResult Display(
    BillingPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<BillingPartViewModel>(
        GetDisplayShapeType(context),
        model =>
        {
          model.Part = part;
          model.Definition = context.TypePartDefinition;
          model.LegalEntityContentItemId =
            part.LegalEntity.ContentItemIds.First();
          model.ContentItemId = part.ContentItem.ContentItemId;
        }
      )
      .Location("Detail", "Content");
  }

  public BillingPartDisplayDriver(
    IServiceProvider serviceProvider,
    IStringLocalizer<BillingPartDisplayDriver> localizer,
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
