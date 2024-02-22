using System.Xml.Linq;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Enms.Abstractions.Models;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Iot.Abstractions.Services;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Scope;

namespace Mess.Enms.Iot;

public class EgaugePushHandler
  : XmlIotPushHandler<EgaugeIotDeviceItem, EgaugeMeasurement>
{
  private readonly IEnmsTimeseriesClient _measurementClient;

  public EgaugePushHandler(
    IEnmsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    EgaugeIotDeviceItem contentItem,
    EgaugeMeasurement request
  )
  {
    _measurementClient.AddEgaugeMeasurement(
      request with
      {
        DeviceId = request.DeviceId,
        Timestamp = request.Timestamp
      }
    );
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    EgaugeIotDeviceItem contentItem,
    EgaugeMeasurement request
  )
  {
    await _measurementClient.AddEgaugeMeasurementAsync(
      request with
      {
        DeviceId = request.DeviceId,
        Timestamp = request.Timestamp
      }
    );
  }

  protected override void HandleBulk(BulkIotXmlPushRequest<EgaugeIotDeviceItem, EgaugeMeasurement>[] requests)
  {
    foreach (var request in requests)
    {
      _measurementClient.AddEgaugeMeasurement(
        request.Request with
        {
          DeviceId = request.DeviceId,
          Timestamp = request.Timestamp
        }
      );
    }
  }

  protected override async Task HandleBulkAsync(BulkIotXmlPushRequest<EgaugeIotDeviceItem, EgaugeMeasurement>[] requests)
  {
    foreach (var request in requests)
    {
      await _measurementClient.AddEgaugeMeasurementAsync(
        request.Request with
        {
          DeviceId = request.DeviceId,
          Timestamp = request.Timestamp
        }
      );
    }
  }

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
      result[registerName] = new EgaugeRegister(type, type.Unit(), value);
    }

    var measurementRegisters = new EgaugeMeasurementRegisters(
      result,
      ShellScope.Current.ShellContext.Settings.Name,
      "egauge",
      timestamp
    );

    var measurement = new EgaugeMeasurement(
      DeviceId: measurementRegisters.Source,
      Timestamp: measurementRegisters.Timestamp,
      Voltage: measurementRegisters.Voltage,
      Power: measurementRegisters.Power
    );

    return measurement;
  }
}
