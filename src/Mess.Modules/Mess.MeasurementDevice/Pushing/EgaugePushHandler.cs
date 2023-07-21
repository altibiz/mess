using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.System.Extensions.Dictionaries;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Pushing;

public class EgaugePushHandler
  : XmlMeasurementDevicePushHandler<EgaugeMeasurement>
{
  public const string PushContentType = "EgaugeMeasurementDevice";

  public override string ContentType => PushContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    EgaugeMeasurement measurement
  ) =>
    _measurementClient.AddEgaugeMeasurement(
      measurement with
      {
        DeviceId = measurement.DeviceId,
        Tenant = measurement.Tenant,
        Timestamp = measurement.Timestamp
      }
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    EgaugeMeasurement measurement
  ) =>
    await _measurementClient.AddEgaugeMeasurementAsync(
      measurement with
      {
        DeviceId = measurement.DeviceId,
        Tenant = measurement.Tenant,
        Timestamp = measurement.Timestamp
      }
    );

  protected override EgaugeMeasurement? Parse(XDocument xml)
  {
    var result = new Dictionary<string, EgaugeRegister>();
    DateTime? timestamp = null;

    var group = xml.Descendants().First();
    var data = group.Descendants().First();
    timestamp = DateTimeOffset
      .FromUnixTimeSeconds(
        Convert.ToInt64(((string?)data.Attribute(XName.Get("time_stamp"))), 16)
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

    var measurementRegisters = new EgaugeMeasurementRegisters(
      Registers: result,
      Tenant: ShellScope.Current.ShellContext.Settings.Name,
      Source: "egauge",
      Timestamp: timestamp.Value
    );

    var measurement = new EgaugeMeasurement(
      DeviceId: measurementRegisters.Source,
      Tenant: measurementRegisters.Tenant,
      Timestamp: measurementRegisters.Timestamp,
      Voltage: measurementRegisters.Voltage,
      Power: measurementRegisters.Power
    );

    return measurement;
  }

  public EgaugePushHandler(
    ILogger<EgaugePushHandler> logger,
    ITimeseriesClient measurementClient
  )
    : base(logger)
  {
    _measurementClient = measurementClient;
  }

  private readonly ITimeseriesClient _measurementClient;
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

public readonly record struct EgaugeMeasurementRegisters(
  IDictionary<string, EgaugeRegister> Registers,
  string Tenant,
  string Source,
  DateTime Timestamp
)
{
  public float Power => (float)Registers.GetOrDefault("P").Value;

  public float Voltage => (float)Registers.GetOrDefault("L1 Voltage").Value;
};
