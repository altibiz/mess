using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Billing;

public interface IOzdsIotDeviceBillingFactory
{
  public bool IsApplicable(ContentItem iotDeviceItem);

  OzdsCalculationData CreateCalculation(
    ContentItem distributionSystemUnitItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<OzdsCalculationData> CreateCalculationAsync(
    ContentItem distributionSystemUnitItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
