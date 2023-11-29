using System.ComponentModel.DataAnnotations.Schema;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Eor.Iot;

public class EorStatusEntity : HypertableEntity
{
  public int Stamp { get; set; } = default!;

  public int Mode { get; set; } = default!;

  public int ProcessFault { get; set; } = default!;

  public string[] ProcessFaults { get; set; } = default!;

  public int CommunicationFault { get; set; } = default!;

  public EorRunState RunState { get; set; } = default!;

  public EorResetState ResetState { get; set; } = default!;

  public EorDoorState DoorState { get; set; } = default!;

  public EorMainCircuitBreakerState MainCircuitBreakerState { get; set; } =
    default!;

  public EorTransformerContractorState TransformerContractorState { get; set; } =
    default!;

  public EorDiodeBridgeState FirstDiodeBridgeState { get; set; } = default!;

  public EorDiodeBridgeState SecondDiodeBridgeState { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float Current { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float Voltage { get; set; } = default!;

  public float Temperature { get; set; } = default!;

  public bool HeatsinkFans { get; set; } = default!;

  public bool CoolingFans { get; set; } = default!;
}

public static class EorStatusEntityExtensions
{
  public static EorStatusEntity ToEntity(this EorStatus model) =>
    new()
    {
      Tenant = model.Tenant,
      Source = model.DeviceId,
      Stamp = model.Stamp,
      Timestamp = model.Timestamp,
      Mode = model.Mode,
      ProcessFault = model.ProcessFault,
      ProcessFaults = model.ProcessFaults,
      CommunicationFault = model.CommunicationFault,
      RunState = model.RunState,
      ResetState = model.ResetState,
      DoorState = model.DoorState,
      MainCircuitBreakerState = model.MainCircuitBreakerState,
      TransformerContractorState = model.TransformerContractorState,
      FirstDiodeBridgeState = model.FirstDiodeBridgeState,
      SecondDiodeBridgeState = model.SecondDiodeBridgeState,
      Current = model.Current,
      Voltage = model.Voltage,
      Temperature = model.Temperature,
      HeatsinkFans = model.HeatsinkFans,
      CoolingFans = model.CoolingFans
    };

  public static EorStatus ToModel(this EorStatusEntity entity) =>
    new(
      Tenant: entity.Tenant,
      DeviceId: entity.Source,
      Stamp: entity.Stamp,
      Timestamp: entity.Timestamp,
      Mode: entity.Mode,
      ProcessFault: entity.ProcessFault,
      ProcessFaults: entity.ProcessFaults,
      CommunicationFault: entity.CommunicationFault,
      RunState: entity.RunState,
      ResetState: entity.ResetState,
      DoorState: entity.DoorState,
      MainCircuitBreakerState: entity.MainCircuitBreakerState,
      TransformerContractorState: entity.TransformerContractorState,
      FirstDiodeBridgeState: entity.FirstDiodeBridgeState,
      SecondDiodeBridgeState: entity.SecondDiodeBridgeState,
      Current: entity.Current,
      Voltage: entity.Voltage,
      Temperature: entity.Temperature,
      HeatsinkFans: entity.HeatsinkFans,
      CoolingFans: entity.CoolingFans
    );
}
