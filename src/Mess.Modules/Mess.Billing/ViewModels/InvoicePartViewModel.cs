using Mess.Billing.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Billing.ViewModels;

public class InvoicePartViewModel
{
  [ValidateNever] public InvoicePart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string ContentItemId { get; set; } = default!;
}
