using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Parsers;

public abstract class MeasurementParser<TModel> : IMeasurementParser
{
  public abstract string Id { get; }

  public abstract string StorageStrategyId { get; }

  public abstract TModel? ParseToModel(string body);

  public abstract Task<TModel?> ParseToModelAsync(string body);

  public virtual ParsedMeasurementModel? Parse(string body)
  {
    var model = ParseToModel(body);
    return model is not null
      ? new ParsedMeasurementModel(StorageStrategyId, model)
      : null;
  }

  public virtual async Task<ParsedMeasurementModel?> ParseAsync(string body)
  {
    var model = await ParseToModelAsync(body);
    return model is not null
      ? new ParsedMeasurementModel(StorageStrategyId, model)
      : null;
  }
}
