using Mess.Eor.Abstractions.Client;

namespace Mess.Eor.MeasurementDevice.Updating;

public record EorUpdateRequest(
  DateTime Timestamp,
  int Mode,
  bool Start,
  bool Stop,
  bool Reset,
  int ProcessFaults,
  int CommunicationFaults,
  bool DoorState,
  bool MainCircuitBreakerState,
  bool TransformerContractorState,
  bool FirstDiodeBridgeState,
  bool SecondDiodeBridgeState,
  float Current,
  float Voltage,
  float Temperature,
  bool HeatsinkFans,
  bool CoolingFans,
  string[] ProcessFaultsArray
)
{
  public EorStatus ToStatus(string tenant, string deviceId) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: Timestamp,
      Mode: Mode,
      ProcessFault: ProcessFaults,
      ProcessFaults: ProcessFaultsArray,
      CommunicationFault: CommunicationFaults,
      RunState: Start && !Stop
        ? EorMeasurementDeviceRunState.Started
        : Stop && !Start
          ? EorMeasurementDeviceRunState.Stopped
          : EorMeasurementDeviceRunState.Error,
      ResetState: Reset
        ? EorMeasurementDeviceResetState.ShouldReset
        : EorMeasurementDeviceResetState.ShouldntReset,
      DoorState: DoorState ? EorDoorState.Open : EorDoorState.Closed,
      MainCircuitBreakerState: MainCircuitBreakerState
        ? EorMainCircuitBreakerState.On
        : EorMainCircuitBreakerState.Off,
      TransformerContractorState: TransformerContractorState
        ? EorTransformerContractorState.On
        : EorTransformerContractorState.Off,
      FirstDiodeBridgeState: FirstDiodeBridgeState
        ? EorDiodeBridgeState.Ok
        : EorDiodeBridgeState.Error,
      SecondDiodeBridgeState: SecondDiodeBridgeState
        ? EorDiodeBridgeState.Ok
        : EorDiodeBridgeState.Error,
      Current: Current,
      Voltage: Voltage,
      Temperature: Temperature,
      HeatsinkFans: HeatsinkFans,
      CoolingFans: CoolingFans
    );
}
