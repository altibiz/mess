using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Mess.Billing.Abstractions.Models;

namespace Mess.Billing.ViewModels;

public class BillingPartViewModel
{
  [ValidateNever]
  public BillingPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string LegalEntityContentItemId { get; set; } = default!;

  public string ContentItemId { get; set; } = default!;
}
