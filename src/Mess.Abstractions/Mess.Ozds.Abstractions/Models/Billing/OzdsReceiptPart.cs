using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsReceiptPart : ContentPart
{
  public OzdsReceiptData Data { get; set; } = default!;
}
