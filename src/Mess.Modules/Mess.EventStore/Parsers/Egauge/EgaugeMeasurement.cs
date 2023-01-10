using Mess.Util.Extensions.IDictionary;

namespace Mess.EventStore.Parsers.Egauge;

public enum EgaugeRegisterType
{
  Irradiance,
  Frequency,
  Current,
  ReactivePower,
  Pressure,
  Power,
  VolumetricFlow,
  MassFlow,
  Resistance,
  ApparentPower,
  TotalHarmonicDistortion,
  Temperature,
  Voltage,
  Numeric,
  Monetary,
  Angle,
  RelativeHumidity,
  Speed,
  Charge
}

// NOTE: this is impossible to implement correctly because some types have the
// same unit
// public static class EgaugeRegisterUnitMethods
// {
//   public static EgaugeRegisterType Type(this EgaugeRegisterUnit unit) =>
//     unit switch
//     {
//       EgaugeRegisterUnit.WattsPerSquareMeter => EgaugeRegisterType.Irradiance,
//       EgaugeRegisterUnit.Hertz => EgaugeRegisterType.Frequency,
//       EgaugeRegisterUnit.Ampere => EgaugeRegisterType.Current,
//       EgaugeRegisterUnit.VoltAmpereReactive => EgaugeRegisterType.ReactivePower,
//       EgaugeRegisterUnit.Pascal => EgaugeRegisterType.Pressure,
//       EgaugeRegisterUnit.Watt => EgaugeRegisterType.Power,
//       EgaugeRegisterUnit.Mm3ps => EgaugeRegisterType.VolumetricFlow,
//       EgaugeRegisterUnit.GramPerSecond => EgaugeRegisterType.MassFlow,
//       EgaugeRegisterUnit.Ohm => EgaugeRegisterType.Resistance,
//       EgaugeRegisterUnit.VoltAmpere => EgaugeRegisterType.ApparentPower,
//       EgaugeRegisterUnit.Percent => EgaugeRegisterType.TotalHarmonicDistortion,
//       EgaugeRegisterUnit.Celsius => EgaugeRegisterType.Temperature,
//       EgaugeRegisterUnit.Volt => EgaugeRegisterType.Voltage,
//       EgaugeRegisterUnit.None => EgaugeRegisterType.Numeric,
//       EgaugeRegisterUnit.CurrencyPerSecond => EgaugeRegisterType.Monetary,
//       EgaugeRegisterUnit.Degrees => EgaugeRegisterType.Angle,
//       EgaugeRegisterUnit.Percent => EgaugeRegisterType.RelativeHumidity,
//       EgaugeRegisterUnit.MeterPerSecond => EgaugeRegisterType.Speed,
//       EgaugeRegisterUnit.AmpereHours => EgaugeRegisterType.Charge,
//       var _ => default
//     };
// }

public enum EgaugeRegisterUnit
{
  WattsPerSquareMeter,
  Hertz,
  Ampere,
  VoltAmpereReactive,
  Pascal,
  Watt,
  Mm3ps,
  GramPerSecond,
  Ohm,
  VoltAmpere,
  Percent,
  Celsius,
  Volt,
  None,
  CurrencyPerSecond,
  Degrees,
  MeterPerSecond,
  AmpereHours
}

public static class EgaugeRegisterTypeMethods
{
  public static EgaugeRegisterUnit Unit(this EgaugeRegisterType type) =>
    type switch
    {
      EgaugeRegisterType.Irradiance => EgaugeRegisterUnit.WattsPerSquareMeter,
      EgaugeRegisterType.Frequency => EgaugeRegisterUnit.Hertz,
      EgaugeRegisterType.Current => EgaugeRegisterUnit.Ampere,
      EgaugeRegisterType.ReactivePower => EgaugeRegisterUnit.VoltAmpereReactive,
      EgaugeRegisterType.Pressure => EgaugeRegisterUnit.Pascal,
      EgaugeRegisterType.Power => EgaugeRegisterUnit.Watt,
      EgaugeRegisterType.VolumetricFlow => EgaugeRegisterUnit.Mm3ps,
      EgaugeRegisterType.MassFlow => EgaugeRegisterUnit.GramPerSecond,
      EgaugeRegisterType.Resistance => EgaugeRegisterUnit.Ohm,
      EgaugeRegisterType.ApparentPower => EgaugeRegisterUnit.VoltAmpere,
      EgaugeRegisterType.TotalHarmonicDistortion => EgaugeRegisterUnit.Percent,
      EgaugeRegisterType.Temperature => EgaugeRegisterUnit.Celsius,
      EgaugeRegisterType.Voltage => EgaugeRegisterUnit.Volt,
      EgaugeRegisterType.Numeric => EgaugeRegisterUnit.None,
      EgaugeRegisterType.Monetary => EgaugeRegisterUnit.CurrencyPerSecond,
      EgaugeRegisterType.Angle => EgaugeRegisterUnit.Degrees,
      EgaugeRegisterType.RelativeHumidity => EgaugeRegisterUnit.Percent,
      EgaugeRegisterType.Speed => EgaugeRegisterUnit.MeterPerSecond,
      EgaugeRegisterType.Charge => EgaugeRegisterUnit.AmpereHours,
      var _ => default
    };
}

public static class EgaugeRegisterTypeString
{
  public const string Irradiance = "Ee";
  public const string Frequency = "F";
  public const string Current = "I";
  public const string ReactivePower = "PQ";
  public const string Pressure = "Pa";
  public const string Power = "P";
  public const string VolumetricFlow = "Qv";
  public const string MassFlow = "Q";
  public const string Resistance = "R";
  public const string ApparentPower = "S";
  public const string TotalHarmonicDistortion = "THD";
  public const string Temperature = "T";
  public const string Voltage = "V";
  public const string Numeric = "#";
  public const string Monetary = "$";
  public const string Angle = "a";
  public const string RelativeHumidity = "h";
  public const string Speed = "v";
  public const string Charge = "Qe";

  public static EgaugeRegisterType? ToEgaugeRegisterType(
    this string egaugeRegisterTypeString
  ) =>
    egaugeRegisterTypeString switch
    {
      Irradiance => EgaugeRegisterType.Irradiance,
      Frequency => EgaugeRegisterType.Frequency,
      Current => EgaugeRegisterType.Current,
      ReactivePower => EgaugeRegisterType.ReactivePower,
      Pressure => EgaugeRegisterType.Pressure,
      Power => EgaugeRegisterType.Power,
      VolumetricFlow => EgaugeRegisterType.VolumetricFlow,
      MassFlow => EgaugeRegisterType.MassFlow,
      Resistance => EgaugeRegisterType.Resistance,
      ApparentPower => EgaugeRegisterType.ApparentPower,
      TotalHarmonicDistortion => EgaugeRegisterType.TotalHarmonicDistortion,
      Temperature => EgaugeRegisterType.Temperature,
      Voltage => EgaugeRegisterType.Voltage,
      Numeric => EgaugeRegisterType.Numeric,
      Monetary => EgaugeRegisterType.Monetary,
      Angle => EgaugeRegisterType.Angle,
      RelativeHumidity => EgaugeRegisterType.RelativeHumidity,
      Speed => EgaugeRegisterType.Speed,
      Charge => EgaugeRegisterType.Charge,
      var _ => default
    };
}

public readonly record struct EgaugeRegister(
  EgaugeRegisterType type,
  EgaugeRegisterUnit unit,
  decimal value
);

public readonly record struct EgaugeMeasurement(
  IDictionary<string, EgaugeRegister> registers,
  DateTime timestamp
)
{
  public float Power => (float)registers.GetOrDefault("P").value;

  public float Voltage => (float)registers.GetOrDefault("L1 Voltage").value;
};
