using System.ComponentModel.DataAnnotations.Schema;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Eor.Iot;

public class EorStatusEntity : HypertableEntity
{
  public int Stamp { get; set; }

  public int Mode { get; set; }

  public int ProcessFault { get; set; }

  public string[] ProcessFaults { get; set; } = default!;

  public int CommunicationFault { get; set; }

  public EorRunState RunState { get; set; }

  public EorResetState ResetState { get; set; }

  public EorDoorState DoorState { get; set; }

  public EorMainCircuitBreakerState MainCircuitBreakerState { get; set; }

  public EorTransformerContractorState TransformerContractorState { get; set; }

  public EorDiodeBridgeState FirstDiodeBridgeState { get; set; }

  public EorDiodeBridgeState SecondDiodeBridgeState { get; set; }

  [Column(TypeName = "float4")] public float Current { get; set; }

  [Column(TypeName = "float4")] public float Voltage { get; set; }

  public float Temperature { get; set; }

  public bool HeatsinkFans { get; set; }

  public bool CoolingFans { get; set; }
}

public static class EorStatusEntityExtensions
{
  public static EorStatusEntity ToEntity(this EorStatus model)
  {
    return new EorStatusEntity
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
  }

  public static EorStatus ToModel(this EorStatusEntity entity)
  {
    return new EorStatus(
      entity.Tenant,
      entity.Source,
      entity.Stamp,
      entity.Timestamp,
      entity.Mode,
      entity.ProcessFault,
      entity.ProcessFaults,
      entity.CommunicationFault,
      entity.RunState,
      entity.ResetState,
      entity.DoorState,
      entity.MainCircuitBreakerState,
      entity.TransformerContractorState,
      entity.FirstDiodeBridgeState,
      entity.SecondDiodeBridgeState,
      entity.Current,
      entity.Voltage,
      entity.Temperature,
      entity.HeatsinkFans,
      entity.CoolingFans
    );
  }
}
