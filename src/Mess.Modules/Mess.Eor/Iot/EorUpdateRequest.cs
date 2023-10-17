using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Iot;

public record EorUpdateRequest(
  DateTimeOffset Timestamp,
  int Stamp,
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
  string ProcessFaultsArray0,
  string ProcessFaultsArray1,
  string ProcessFaultsArray2,
  string ProcessFaultsArray3,
  string ProcessFaultsArray4,
  string ProcessFaultsArray5,
  string ProcessFaultsArray6,
  string ProcessFaultsArray7,
  string ProcessFaultsArray8,
  string ProcessFaultsArray9,
  string ProcessFaultsArray10,
  string ProcessFaultsArray11,
  string ProcessFaultsArray12,
  string ProcessFaultsArray13,
  string ProcessFaultsArray14,
  string ProcessFaultsArray15
)
{
  public EorStatus ToStatus(string tenant, string deviceId) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Stamp: Stamp,
      Timestamp: Timestamp,
      Mode: Mode,
      ProcessFault: ProcessFaults,
      ProcessFaults: new[]
      {
        ProcessFaultsArray0,
        ProcessFaultsArray1,
        ProcessFaultsArray2,
        ProcessFaultsArray3,
        ProcessFaultsArray4,
        ProcessFaultsArray5,
        ProcessFaultsArray6,
        ProcessFaultsArray7,
        ProcessFaultsArray8,
        ProcessFaultsArray9,
        ProcessFaultsArray10,
        ProcessFaultsArray11,
        ProcessFaultsArray12,
        ProcessFaultsArray13,
        ProcessFaultsArray14,
        ProcessFaultsArray15
      },
      CommunicationFault: CommunicationFaults,
      RunState: Start && !Stop
        ? EorRunState.Started
        : Stop && !Start
          ? EorRunState.Stopped
          : EorRunState.Error,
      ResetState: Reset
        ? EorResetState.ShouldReset
        : EorResetState.ShouldntReset,
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
