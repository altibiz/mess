using Mess.MeasurementDevice.Pushing;
using Mess.MeasurementDevice.Abstractions.Client;
using Moq;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Test;

public record class EgaugePushHandlerTest(
  EgaugePushHandler Handler,
  Mock<ITimeseriesClient> MeasurementClientMock,
  ILogger<EgaugePushHandlerTest> Logger
)
{
  [Theory]
  [StaticData(typeof(Assets), nameof(Assets.EgaugeMeasurements))]
  public void HandleTest(string request, EgaugeMeasurement measurement)
  {
    Handler.Handle("", new ContentItem(), request);

    MeasurementClientMock.Verify(
      client => client.AddEgaugeMeasurement(measurement),
      Times.Once
    );
  }
}
