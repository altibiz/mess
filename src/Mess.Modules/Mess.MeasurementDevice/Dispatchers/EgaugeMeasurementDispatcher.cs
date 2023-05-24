using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Mess.Tenants;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.System.Extensions.Objects;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.System.Extensions.Dictionaries;

namespace Mess.MeasurementDevice.Dispatchers;

public record class EgaugeMeasurementDispatcher(
  ILogger<EgaugeMeasurementDispatcher> Logger,
  ITenants Tenant,
  IMeasurementClient Client
) : IMeasurementDispatcher
{
  public const string ParserId = "egauge";

  public string Id => ParserId;

  public void Dispatch(string measurement)
  {
    var xml = measurement.FromXml();
    if (xml is null)
    {
      return;
    }

    var result = new Dictionary<string, EgaugeRegister>();
    DateTime? timestamp = null;

    try
    {
      var group = xml.Descendants().First();
      var data = group.Descendants().First();
      timestamp = DateTimeOffset
        .FromUnixTimeSeconds(
          Convert.ToInt64(
            ((string?)data.Attribute(XName.Get("time_stamp"))),
            16
          )
        )
        .UtcDateTime;
      var registers = data.Descendants(XName.Get("cname"));
      var columns = data.Descendants(XName.Get("r")).First().Descendants();

      foreach (var (register, column) in registers.Zip(columns))
      {
        var registerName = (string)register;
        var type =
          ((string?)register.Attribute(XName.Get("t")))?.ToEgaugeRegisterType()
          ?? throw new ArgumentException(
            "Register {registerName} has no type",
            registerName
          );
        var value = (decimal)column;
        result[registerName] = new(type, type.Unit(), value);
      }
    }
    catch (Exception exception)
    {
      Logger.LogError(exception, "Failed parsing xml {file}", xml.BaseUri);
      return;
    }

    var parsedModel = new ParsedEgaugeMeasurementModel(
      Registers: result,
      Tenant: Tenant.Current.Name,
      Source: "egauge",
      Timestamp: timestamp.Value
    );

    var model = new DispatchedEgaugeMeasurement(
      Source: parsedModel.Source,
      Tenant: parsedModel.Tenant,
      Timestamp: parsedModel.Timestamp,
      Voltage: parsedModel.Voltage,
      Power: parsedModel.Power
    );

    Client.AddEgaugeMeasurement(model);
  }

  public async Task DispatchAsync(string measurement)
  {
    var xml = measurement.FromXml();
    if (xml is null)
    {
      return;
    }

    var result = new Dictionary<string, EgaugeRegister>();
    DateTime? timestamp = null;

    try
    {
      var group = xml.Descendants().First();
      var data = group.Descendants().First();
      timestamp = DateTimeOffset
        .FromUnixTimeSeconds(
          Convert.ToInt64(
            ((string?)data.Attribute(XName.Get("time_stamp"))),
            16
          )
        )
        .UtcDateTime;
      var registers = data.Descendants(XName.Get("cname"));
      var columns = data.Descendants(XName.Get("r")).First().Descendants();

      foreach (var (register, column) in registers.Zip(columns))
      {
        var registerName = (string)register;
        var type =
          ((string?)register.Attribute(XName.Get("t")))?.ToEgaugeRegisterType()
          ?? throw new ArgumentException(
            "Register {registerName} has no type",
            registerName
          );
        var value = (decimal)column;
        result[registerName] = new(type, type.Unit(), value);
      }
    }
    catch (Exception exception)
    {
      Logger.LogError(exception, "Failed parsing xml {file}", xml.BaseUri);
      return;
    }

    var parsedModel = new ParsedEgaugeMeasurementModel(
      Registers: result,
      Tenant: Tenant.Current.Name,
      Source: "egauge",
      Timestamp: timestamp.Value
    );

    var model = new DispatchedEgaugeMeasurement(
      Source: parsedModel.Source,
      Tenant: parsedModel.Tenant,
      Timestamp: parsedModel.Timestamp,
      Voltage: parsedModel.Voltage,
      Power: parsedModel.Power
    );

    await Client.AddEgaugeMeasurementAsync(model);
  }
}

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
  EgaugeRegisterType Type,
  EgaugeRegisterUnit Unit,
  decimal Value
);

public readonly record struct ParsedEgaugeMeasurementModel(
  IDictionary<string, EgaugeRegister> Registers,
  string Tenant,
  string Source,
  DateTime Timestamp
)
{
  public float Power => (float)Registers.GetOrDefault("P").Value;

  public float Voltage => (float)Registers.GetOrDefault("L1 Voltage").Value;
};
