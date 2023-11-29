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
          model.ContentItemId = part.ContentItem.ContentItemId;
        }
      )
      .Location("Detail", "Content");
  }
}
