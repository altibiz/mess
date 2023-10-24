using System.Xml.Linq;
using Mess.Iot.Abstractions.Services;
using Mess.Enms.Abstractions.Timeseries;
using OrchardCore.Environment.Shell.Scope;
using Mess.Enms.Abstractions.Models;

namespace Mess.Enms.Iot;

public class EgaugePushHandler
  : XmlIotPushHandler<EgaugeIotDeviceItem, EgaugeMeasurement>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EgaugeIotDeviceItem contentItem,
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
    DateTimeOffset timestamp,
    EgaugeIotDeviceItem contentItem,
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

  protected override EgaugeMeasurement Parse(XDocument xml)
  {
    var result = new Dictionary<string, EgaugeRegister>();

    var group = xml.Descendants().First();
    var data = group.Descendants().First();
    var timestamp = DateTimeOffset.FromUnixTimeSeconds(
      Convert.ToInt64((string?)data.Attribute(XName.Get("time_stamp")), 16)
    );
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
      Timestamp: timestamp
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

  public EgaugePushHandler(IEnmsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  private readonly IEnmsTimeseriesClient _measurementClient;
}
