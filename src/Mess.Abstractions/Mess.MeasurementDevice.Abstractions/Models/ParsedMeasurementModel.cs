namespace Mess.MeasurementDevice.Abstractions.Models;

public record struct ParsedMeasurementModel(
  string StorageStrategy,
  object Model
);
