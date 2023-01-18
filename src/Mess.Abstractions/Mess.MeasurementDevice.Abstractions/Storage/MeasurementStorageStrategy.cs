using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Storage;

public abstract record class MeasurementStorageStrategy<TModel>
  : IMeasurementStorageStrategy
{
  public abstract string Id { get; }

  public abstract string? StoreModel(TModel model);

  public abstract Task<string?> StoreModelAsync(TModel model);

  public virtual string? Store(ParsedMeasurementModel parsedMeasurement)
  {
    var model = (TModel)parsedMeasurement.Model;
    return model is not null ? StoreModel(model) : "Invalid type of model";
  }

  public virtual async Task<string?> StoreAsync(
    ParsedMeasurementModel parsedMeasurement
  )
  {
    var model = (TModel)parsedMeasurement.Model;
    return model is not null
      ? await StoreModelAsync(model)
      : "Invalid type of model";
  }
}
