using Mess.Iot.Iot;
using Mess.Iot.Abstractions.Timeseries;
using Moq;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Test;

public record class EgaugePushHandlerTest(
  EgaugePushHandler Handler,
  Mock<IIotTimeseriesClient> MeasurementClientMock,
  ILogger<EgaugePushHandlerTest> Logger
)
{
  [Theory]
  [StaticData(typeof(Assets), nameof(Assets.EgaugeMeasurements))]
  public void HandleTest(string request, EgaugeMeasurement measurement)
  {
    Handler.Handle(
      measurement.DeviceId,
      measurement.Tenant,
      measurement.Timestamp,
      new ContentItem(),
      request
    );

    MeasurementClientMock.Verify(
      client => client.AddEgaugeMeasurement(measurement),
      Times.Once
    );
  }
}
