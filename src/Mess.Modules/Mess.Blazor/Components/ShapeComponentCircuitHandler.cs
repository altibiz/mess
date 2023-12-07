using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Mess.Blazor.Abstractions.Components;

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

  public override async Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    await base.OnCircuitClosedAsync(circuit, cancellationToken);

    _shapeComponentModelStore.Remove(circuit.Id);
  }

  public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
  {
    await base.OnCircuitOpenedAsync(circuit, cancellationToken);

    _shapeComponentCircuitAccessor.Circuit = circuit;
  }
}
