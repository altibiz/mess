using OrchardCore.ContentManagement;

namespace Mess.Fields.Abstractions.Fields;

public class IntervalField : ContentField
{
  public Interval Value { get; set; } = default!;
}
