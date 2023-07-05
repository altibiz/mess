namespace Mess.Eor.Abstractions.Client;

public record EorStatus(
  string Tenant,
  string DeviceId,
  DateTime Timestamp,
  int Mode,
  int ProcessFaults,
  int CommunicationFaults,
  EorMeasurementDeviceRunState RunState,
  EorMeasurementDeviceResetState ResetState,
  EorDoorState DoorState,
  EorMainCircuitBreakerState MainCircuitBreakerState,
  EorTransformerContractorState TransformerContractorState,
  EorDiodeBridgeState FirstDiodeBridgeState,
  EorDiodeBridgeState SecondDiodeBridgeState
)
{
  public EorMeasurementDevicePowerState PowerState =>
    ProcessFaults == ((int)EorMeasurementDevicePowerState.On)
      ? EorMeasurementDevicePowerState.On
      : ProcessFaults == ((int)EorMeasurementDevicePowerState.Off)
        ? EorMeasurementDevicePowerState.Off
        : EorMeasurementDevicePowerState.Error;

  public string? Error =>
    ProcessFaults != 0
      ? "ProcessFaults"
      : CommunicationFaults != 0
        ? "CommunicationFaults"
        : null;
}

public enum EorMeasurementDeviceRunState : int
{
  Stopped = default,
  Started = 1,
};

public enum EorMeasurementDeviceResetState : int
{
  ShouldntReset = default,
  ShouldReset = 1,
};

public enum EorMeasurementDevicePowerState : int
{
  On = default,
  Off = 999,
  Error = 1,
};

public enum EorDoorState : int
{
  Closed = default,
  Open = 1,
};

public enum EorMainCircuitBreakerState : int
{
  On = default,
  Off = 1,
};

public enum EorTransformerContractorState : int
{
  On = default,
  Off = 1,
};

public enum EorDiodeBridgeState : int
{
  Error = default,
  Ok = 1,
};
