using Mess.MeasurementDevice.Abstractions.Models;
using Mess.System;

namespace Mess.MeasurementDevice.Abstractions.Parsers;

public interface IMeasurementParser
{
  public string Id { get; }

  public ParsedMeasurementModel? Parse(string body);

  public Task<ParsedMeasurementModel?> ParseAsync(string body);
}
