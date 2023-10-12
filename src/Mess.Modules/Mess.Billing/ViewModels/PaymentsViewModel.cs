using OrchardCore.ContentManagement;

namespace Mess.Billing.ViewModels;

public class PaymentsViewModel
{
  public List<PaymentViewModel> Payments { get; set; } = default!;
}

public class PaymentViewModel
{
  public ContentItem InvoiceItem { get; set; } = default!;

  public ContentItem? ReceiptItem { get; set; } = default!;
}
