using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.MeasurementDevice.Dispatchers;
using Mess.MeasurementDevice.Abstractions.Client;
using Moq;

namespace Mess.MeasurementDevice.Test;

public record class EgaugeMeasurementDispatcherTest(
  ITenantFixture Fixture,
  EgaugeMeasurementDispatcher Dispatcher,
  Mock<IMeasurementClient> MeasurementClientMock,
  ILogger<EgaugeMeasurementDispatcherTest> Logger
)
{
  [Theory]
  [StaticData(typeof(Assets), nameof(Assets.EgaugeMeasurements))]
  public void DispatchTest(
    string unparsedMeasurement,
    DispatchedEgaugeMeasurement model
  )
  {
    Dispatcher.Dispatch(unparsedMeasurement);
    MeasurementClientMock.Verify(
      client => client.AddEgaugeMeasurement(model),
      Times.Once
    );
  }
}
