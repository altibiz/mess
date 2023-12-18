using Mess.Blazor.Abstractions.Components;

namespace Mess.Blazor.Components;

public class CaptureIdAccessor : ICaptureIdAccessor
{
  public Guid? CaptureId { get; set; }
}
