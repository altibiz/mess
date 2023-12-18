namespace Mess.Blazor.Abstractions.Components;

public interface ICaptureIdAccessor
{
  public Guid? CaptureId { get; set; }
}
