using Mess.System.Extensions.Object;
using Mess.MeasurementDevice.Test.Assets;
using Mess.MeasurementDevice.Parsers.Egauge;
using Mess.MeasurementDevice.Abstractions.Models;
using OrchardCore.Environment.Shell;
using Moq;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Environment.Extensions.Features;
using Mess.MeasurementDevice.EventStore.Storage;
using Xunit.DependencyInjection;
using Mess.MeasurementDevice.Storage;

namespace Mess.MeasurementDevice.Test;

[Startup(typeof(Startup), Shared = false)]
public record class EgaugeDirectStorageParserTest(
  EgaugeParser Parser,
  ITenants Tenants,
  ILogger<EgaugeDirectStorageParserTest> Logger
)
{
  [Theory]
  [StaticData(typeof(EgaugeAssets), nameof(EgaugeAssets.Measurement))]
  public void ParseToEventStoreTest(string unparsedMeasurement)
  {
    var measurement = Parser.Parse(unparsedMeasurement);
    Assert.NotNull(measurement);
    Assert.Equal(
      EgaugeDirectStorageStrategy.StorageStrategyId,
      measurement.Value.StorageStrategy
    );
    Logger.LogInformation(measurement.ToJson());

    var model = (EgaugeMeasurementModel)measurement.Value.Model;
    Logger.LogInformation(model.ToJson());
    Assert.Equal(Tenants.Current.Name, model.Tenant);
    Assert.NotEmpty(model.Source);
    Assert.True(model.Timestamp.Year > 2000);
    Assert.NotEqual(0, model.Power);
    Assert.NotEqual(0, model.Voltage);
  }

  public class Startup : Test.Startup
  {
    public override void ConfigureServices(
      IServiceCollection services,
      HostBuilderContext hostBuilderContext
    )
    {
      base.ConfigureServices(services, hostBuilderContext);

      services.AddSingleton(services =>
      {
        var shellFeaturesManager = new Mock<IShellFeaturesManager>();
        shellFeaturesManager
          .Setup(x => x.GetEnabledFeaturesAsync())
          .Returns(Task.FromResult(Enumerable.Empty<IFeatureInfo>()));
        return shellFeaturesManager.Object;
      });
    }
  }
}
