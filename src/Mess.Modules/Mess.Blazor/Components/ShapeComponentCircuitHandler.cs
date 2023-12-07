using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Mess.Blazor.Components;

public class ShapeComponentCircuitHandler : CircuitHandler
{
  private readonly ShapeComponentCircuitAccessor _shapeComponentCircuitAccessor;
  private readonly IShapeComponentModelStore _shapeComponentModelStore;

  public ShapeComponentCircuitHandler(
    ShapeComponentCircuitAccessor shapeComponentCircuitAccessor,
    IShapeComponentModelStore shapeComponentModelStore
  )
  {
    _shapeComponentCircuitAccessor = shapeComponentCircuitAccessor;
    _shapeComponentModelStore = shapeComponentModelStore;
  }

  public override async Task OnCircuitOpenedAsync(Circuit circuit,
    CancellationToken cancellationToken)
  {
    await base.OnCircuitOpenedAsync(circuit, cancellationToken);

    _shapeComponentCircuitAccessor.Circuit = circuit;
  }

  public override async Task OnCircuitClosedAsync(Circuit circuit,
    CancellationToken cancellationToken)
  {
    await base.OnCircuitClosedAsync(circuit, cancellationToken);

    _shapeComponentModelStore.Remove(circuit.Id);
  }
}
