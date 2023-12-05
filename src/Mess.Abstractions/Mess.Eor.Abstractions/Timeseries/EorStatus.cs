namespace Mess.Eor.Abstractions.Timeseries;

public record EorStatus(
  string Tenant,
  string DeviceId,
  int Stamp,
  DateTimeOffset Timestamp,
  int Mode,
  int ProcessFault,
  string[] ProcessFaults,
  int CommunicationFault,
  EorRunState RunState,
  EorResetState ResetState,
  EorDoorState DoorState,
  EorMainCircuitBreakerState MainCircuitBreakerState,
  EorTransformerContractorState TransformerContractorState,
  EorDiodeBridgeState FirstDiodeBridgeState,
  EorDiodeBridgeState SecondDiodeBridgeState,
  float Current,
  float Voltage,
  float Temperature,
  bool HeatsinkFans,
  bool CoolingFans
);

public enum EorRunState
{
  Stopped = default,
  Started = 1,
  Error = 2
}

public enum EorResetState
{
  ShouldntReset = default,
  ShouldReset = 1
}

public enum EorPowerState
{
  On = default,
  Off = 999,
  Error = 1
}

public enum EorDoorState
{
  Closed = default,
  Open = 1
}

public enum EorMainCircuitBreakerState
{
  Off = default,
  On = 1
}

public enum EorTransformerContractorState
{
  On = default,
  Off = 1
}

public enum EorDiodeBridgeState
{
  Error = default,
  Ok = 1
}
