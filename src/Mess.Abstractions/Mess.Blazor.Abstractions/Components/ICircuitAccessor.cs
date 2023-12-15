using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Mess.Blazor.Abstractions.Components;

public interface ICircuitAccessor
{
  public Circuit? Circuit { get; }
}
