namespace Mess.Timeseries.Abstractions.Entities;

public record class Measurement(
  string tenant,
  string sourceId,
  DateTime timestamp,
  float power,
  float voltage
);
