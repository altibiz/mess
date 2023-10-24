using OrchardCore.ContentManagement;

namespace Mess.Billing.ViewModels;

public class BillsViewModel
{
  public List<BillViewModel> Bills { get; set; } = default!;
}

public class BillViewModel
{
  public ContentItem InvoiceItem { get; set; } = default!;

  public ContentItem? ReceiptItem { get; set; } = default!;
}
