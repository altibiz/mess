using Mess.Enms.Abstractions.Timeseries;
using Mess.Enms.Test;
using Moq;
using OrchardCore.ContentManagement;

namespace Mess.Enms.Iot.Test;

public record class EgaugePushHandlerTest(
  EgaugePushHandler Handler,
  Mock<IEnmsTimeseriesClient> TimeseriesClientMock,
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

    TimeseriesClientMock.Verify(
      client => client.AddEgaugeMeasurement(measurement),
      Times.Once
    );
  }
}
