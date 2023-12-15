using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Mess.Blazor.Components;

public class CircuitAccessor : ICircuitAccessor
{
  public Circuit Circuit { get; set; } = default!;
}
