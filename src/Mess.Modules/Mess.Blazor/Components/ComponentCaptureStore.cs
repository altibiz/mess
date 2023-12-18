using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Mess.Prelude.Extensions.Dictionaries;

// TODO: keepalive for storage so its not a memory leak in case of failure

namespace Mess.Blazor.Components;

public class ComponentCaptureStore : IComponentCaptureStore
{
  private record ShapeComponentCircuitCapture
  {
    public ComponentCapture Capture { get; set; } = default!;

    public string? CircuitId { get; set; } = default!;
  }

  private readonly Dictionary<Guid, ShapeComponentCircuitCapture> _captures = new();

  public void Add(Guid renderId, ComponentCapture? capture)
  {
    if (capture is null) return;

    lock (_captures)
    {
      _captures.Add(renderId, new ShapeComponentCircuitCapture
      {
        Capture = capture,
      });
    }
  }

  public ComponentCapture? Get(Guid renderId, string? circuitId)
  {
    lock (_captures)
    {
      var capture = _captures.GetOrDefault(renderId);

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
      var captureRenderIdsToRemove = new List<Guid>();
      foreach (var pair in _captures)
      {
        if (pair.Value.CircuitId == circuitId)
        {
          captureRenderIdsToRemove.Add(pair.Key);
        }
      }

      foreach (var renderId in captureRenderIdsToRemove)
      {
        _captures.Remove(renderId);
      }
    }
  }
}
