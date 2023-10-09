using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Client;

public interface IOzdsQuery
{
  public Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );

  public OzdsBillingData? GetAbbBillingData(
    string source,
    DateTime beginning,
    DateTime end
  );

  public Task<OzdsBillingData?> GetAbbBillingDataAsync(
    string source,
    DateTime beginning,
    DateTime end
  );
}
