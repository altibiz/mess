using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Storage;

namespace Mess.MeasurementDevice.Storage;

public record class EgaugeDirectStorageStrategy(IMeasurementClient client)
  : MeasurementStorageStrategy<EgaugeMeasurementModel>
{
  public const string StorageStrategyId = "EgaugeEventStorage";

  public override string Id => StorageStrategyId;

  public override string? StoreModel(EgaugeMeasurementModel model)
  {
    client.AddEgaugeMeasurement(model);
    return null;
  }

  public override async Task<string?> StoreModelAsync(
    EgaugeMeasurementModel model
  )
  {
    await client.AddEgaugeMeasurementAsync(model);
    return null;
  }
}
