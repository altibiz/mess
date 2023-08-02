using OrchardCore.ContentManagement;

namespace Mess.ContentFields.Abstractions.Fields;

public class IntervalField : ContentField
{
  public Interval Value { get; set; } = default!;
}
