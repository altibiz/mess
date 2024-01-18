using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsInvoicePart : ContentPart
{

  public OzdsInvoiceData Data { get; set; } = default!;
}
