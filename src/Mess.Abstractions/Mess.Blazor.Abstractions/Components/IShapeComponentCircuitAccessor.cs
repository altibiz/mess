using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Mess.Blazor.Abstractions.Components;

public interface IShapeComponentCircuitAccessor
{
  public Circuit? Circuit { get; }
}
