using Microsoft.AspNetCore.Components.Server.Circuits;
using Mess.Blazor.Abstractions.Components;

namespace Mess.Blazor.Components;

public class ShapeComponentCircuitAccessor : IShapeComponentCircuitAccessor
{
  public Circuit Circuit { get; set; } = default!;
}
