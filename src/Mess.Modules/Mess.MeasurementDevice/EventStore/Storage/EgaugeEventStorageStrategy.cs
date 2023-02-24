using Mess.EventStore.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Storage;
using Mess.MeasurementDevice.EventStore.Events;

namespace Mess.MeasurementDevice.EventStore.Storage;

public record class EgaugeEventStoreStorageStrategy(IEventStoreClient client)
  : MeasurementStorageStrategy<EgaugeMeasurementModel>
{
  public const string StorageStrategyId = "EgaugeEventStorage";

  public override string Id => StorageStrategyId;

  public override string? StoreModel(EgaugeMeasurementModel model)
  {
    client.RecordEvents<MeasurementStream>(new EgaugeMeasured(model));
    return null;
  }

  public override async Task<string?> StoreModelAsync(
    EgaugeMeasurementModel model
  )
  {
    await client.RecordEventsAsync<MeasurementStream>(
      new EgaugeMeasured(model)
    );
    return null;
  }
}
