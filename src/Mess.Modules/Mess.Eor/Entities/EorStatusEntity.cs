using System.ComponentModel.DataAnnotations.Schema;
using Mess.Eor.Abstractions.Client;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Eor.Entities;

public class EorStatusEntity : HypertableEntity
{
  public int Mode { get; set; } = default!;

  public int CommunicationFaults { get; set; } = default!;

  public int ProcessFaults { get; set; } = default!;

  public EorMeasurementDeviceRunState RunState { get; set; } = default!;

  public EorMeasurementDeviceResetState ResetState { get; set; } = default!;

  public EorDoorState DoorState { get; set; } = default!;

  public EorMainCircuitBreakerState MainCircuitBreakerState { get; set; } =
    default!;

  public EorTransformerContractorState TransformerContractorState { get; set; } =
    default!;

  public EorDiodeBridgeState FirstDiodeBridgeState { get; set; } = default!;

  public EorDiodeBridgeState SecondDiodeBridgeState { get; set; } = default!;
}

public static class EorStatusEntityExtensions
{
  public static EorStatusEntity ToEntity(this EorStatus model) =>
    new EorStatusEntity
    {
      Tenant = model.Tenant,
      Source = model.DeviceId,
      Timestamp = model.Timestamp,
      Mode = model.Mode,
      CommunicationFaults = model.CommunicationFaults,
      ProcessFaults = model.ProcessFaults,
      RunState = model.RunState,
      ResetState = model.ResetState,
      DoorState = model.DoorState,
      MainCircuitBreakerState = model.MainCircuitBreakerState,
      TransformerContractorState = model.TransformerContractorState,
      FirstDiodeBridgeState = model.FirstDiodeBridgeState,
      SecondDiodeBridgeState = model.SecondDiodeBridgeState
    };

  public static EorStatus ToModel(this EorStatusEntity entity) =>
    new EorStatus(
      Tenant: entity.Tenant,
      DeviceId: entity.Source,
      Timestamp: entity.Timestamp,
      Mode: entity.Mode,
      CommunicationFaults: entity.CommunicationFaults,
      ProcessFaults: entity.ProcessFaults,
      RunState: entity.RunState,
      ResetState: entity.ResetState,
      DoorState: entity.DoorState,
      MainCircuitBreakerState: entity.MainCircuitBreakerState,
      TransformerContractorState: entity.TransformerContractorState,
      FirstDiodeBridgeState: entity.FirstDiodeBridgeState,
      SecondDiodeBridgeState: entity.SecondDiodeBridgeState
    );
}
