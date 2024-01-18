using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Services;

public interface IOzdsIotDeviceBillingFactory
{
  public bool IsApplicable(ContentItem iotDeviceItem);

  OzdsCalculationData CreateCalculation(
    DistributionSystemUnitItem distributionSystemUnitItem,
    ClosedDistributionSystemItem closedDistributionSystemItem,
    DistributionSystemOperatorItem distributionSystemOperatorItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<OzdsCalculationData> CreateCalculationAsync(
    DistributionSystemUnitItem distributionSystemUnitItem,
    ClosedDistributionSystemItem closedDistributionSystemItem,
    DistributionSystemOperatorItem distributionSystemOperatorItem,
    RegulatoryAgencyCatalogueItem regulatoryAgencyCatalogueItem,
    ContentItem iotDeviceItem,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
