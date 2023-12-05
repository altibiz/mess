using Mess.Prelude.Extensions.Dictionaries;

namespace Mess.Enms.Iot;

public readonly record struct EgaugeMeasurementRegisters(
  IDictionary<string, EgaugeRegister> Registers,
  string Tenant,
  string Source,
  DateTimeOffset Timestamp
)
{
  public float Power => (float)Registers.GetOrDefault("P").Value;

  public float Voltage => (float)Registers.GetOrDefault("L1 Voltage").Value;
}
