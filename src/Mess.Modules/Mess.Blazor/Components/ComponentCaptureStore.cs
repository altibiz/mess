using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Mess.Prelude.Extensions.Dictionaries;

namespace Mess.Blazor.Components;

public class ComponentCaptureStore : IComponentCaptureStore
{
  private record ShapeComponentCircuitCapture
  {
    public ComponentCapture Capture { get; set; } = default!;

    public string? CircuitId { get; set; } = default!;
  }

  private readonly Dictionary<Guid, ShapeComponentCircuitCapture> _captures = new();

  public void Add(Guid captureId, ComponentCapture? capture)
  {
    if (capture is null) return;

    lock (_captures)
    {
      _captures.Add(captureId, new ShapeComponentCircuitCapture
      {
        Capture = capture,
      });
    }
  }

  public ComponentCapture? Get(Guid captureId, string? circuitId)
  {
    lock (_captures)
    {
      var capture = _captures.GetOrDefault(captureId);

      if (capture is not null)
      {
        capture.CircuitId ??= circuitId;
        return capture.Capture;
      }
    }

    return null;
  }

  public void Remove(string circuitId)
  {
    lock (_captures)
    {
      var captureCaptureIdsToRemove = new List<Guid>();
      foreach (var pair in _captures)
      {
        if (pair.Value.CircuitId == circuitId)
        {
          captureCaptureIdsToRemove.Add(pair.Key);
        }
      }

      foreach (var captureId in captureCaptureIdsToRemove)
      {
        _captures.Remove(captureId);
      }
    }
  }
}
