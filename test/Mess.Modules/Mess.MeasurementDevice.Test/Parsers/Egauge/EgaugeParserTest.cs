using Mess.EventStore.Abstractions.Parsers.Egauge;
using System.Xml.Linq;
using Mess.System.Extensions.IDictionary;
using Mess.System.Extensions.Object;
using Mess.MeasurementDevice.Test.Assets;

namespace Mess.MeasurementDevice.Test;

public record class EgaugeParserTest(
  IEgaugeParser parser,
  ILogger<EgaugeParserTest> logger
)
{
  [Theory]
  [StaticData(typeof(EgaugeAssets), nameof(EgaugeAssets.Measurement))]
  public void ParseTest(XDocument xml)
  {
    var measurement = parser.Parse(xml);

    Assert.NotNull(measurement);
    logger.LogInformation(measurement.ToJson());
    Assert.NotNull(measurement);
    Assert.Equal(65, measurement?.registers.Count);

    var powerRegister = measurement?.registers.GetOrDefault("P");
    Assert.NotNull(powerRegister);
    Assert.Equal(EgaugeRegisterType.Power, powerRegister?.type);
    Assert.Equal(EgaugeRegisterUnit.Watt, powerRegister?.unit);

    var voltageRegister = measurement?.registers.GetOrDefault("L1 Voltage");
    Assert.NotNull(voltageRegister);
    Assert.Equal(EgaugeRegisterType.Voltage, voltageRegister?.type);
    Assert.Equal(EgaugeRegisterUnit.Volt, voltageRegister?.unit);

    var timestamp = measurement?.timestamp;
    Assert.NotNull(timestamp);
    Assert.True(timestamp?.Year > 2000);
  }
}
