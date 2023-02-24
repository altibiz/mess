using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Parsers;

public interface IMeasurementParser
{
  public string Id { get; }

  public ParsedMeasurementModel? Parse(string body);

  public Task<ParsedMeasurementModel?> ParseAsync(string body);
}
