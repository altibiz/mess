using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Mess.Tenants;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.System.Extensions.Object;
using Mess.MeasurementDevice.Abstractions.Parsers;
using OrchardCore.Environment.Shell;
using Mess.MeasurementDevice.Storage;
using Mess.MeasurementDevice.EventStore.Storage;

namespace Mess.MeasurementDevice.Parsers.Egauge;

public record class EgaugeParser(
  ILogger<EgaugeParser> Logger,
  ITenants Tenant,
  IShellFeaturesManager Features
) : IMeasurementParser
{
  public const string ParserId = "Egauge";

  public string Id => ParserId;

  public ParsedMeasurementModel? Parse(string body)
  {
    var xml = body.FromXml();
    if (xml is null)
    {
      return null;
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
      return default;
    }

    var parsedModel = new ParsedEgaugeMeasurementModel(
      Registers: result,
      Tenant: Tenant.Current.Name,
      Source: "egauge", // TODO: extract source
      Timestamp: timestamp.Value
    );

    var features = Features.GetEnabledFeaturesAsync().Result;
    var storageStrategyId = features.Any(
      feature => feature.Name == "Mess.EventStore"
    )
      ? EgaugeEventStoreStorageStrategy.StorageStrategyId
      : EgaugeDirectStorageStrategy.StorageStrategyId;

    return new(
      storageStrategyId,
      new EgaugeMeasurementModel(
        Source: parsedModel.Source,
        Tenant: parsedModel.Tenant,
        Timestamp: parsedModel.Timestamp,
        Voltage: parsedModel.Voltage,
        Power: parsedModel.Voltage
      )
    );
  }

  public async Task<ParsedMeasurementModel?> ParseAsync(string body)
  {
    var xml = body.FromXml();
    if (xml is null)
    {
      return null;
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
      return default;
    }

    var parsedModel = new ParsedEgaugeMeasurementModel(
      Registers: result,
      Tenant: Tenant.Current.Name,
      Source: "egauge", // TODO: extract source
      Timestamp: timestamp.Value
    );

    var storageStrategyId = (await Features.GetEnabledFeaturesAsync()).Any(
      feature => feature.Name == "Mess.EventStore"
    )
      ? EgaugeDirectStorageStrategy.StorageStrategyId
      : EgaugeEventStoreStorageStrategy.StorageStrategyId;

    return new(
      storageStrategyId,
      new EgaugeMeasurementModel(
        Source: parsedModel.Source,
        Tenant: parsedModel.Tenant,
        Timestamp: parsedModel.Timestamp,
        Voltage: parsedModel.Voltage,
        Power: parsedModel.Voltage
      )
    );
  }
}
