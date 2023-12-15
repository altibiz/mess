using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Mess.Blazor.Components;

public class ComponentCaptureCircuitHandler : CircuitHandler
{
  private readonly CircuitAccessor _circuitAccessor;
  private readonly IComponentCaptureStore _componentCaptureStore;

  public ComponentCaptureCircuitHandler(
    CircuitAccessor shapeComponentCircuitAccessor,
    IComponentCaptureStore componentCaptureStore
  )
  {
    _circuitAccessor = shapeComponentCircuitAccessor;
    _componentCaptureStore = componentCaptureStore;
  }

  public override async Task OnCircuitOpenedAsync(Circuit circuit,
    CancellationToken cancellationToken)
  {
    await base.OnCircuitOpenedAsync(circuit, cancellationToken);

    _circuitAccessor.Circuit = circuit;
  }

  public override async Task OnCircuitClosedAsync(Circuit circuit,
    CancellationToken cancellationToken)
  {
    await base.OnCircuitClosedAsync(circuit, cancellationToken);

    _componentCaptureStore.Remove(circuit.Id);
  }
}
