using Mess.EventStore.Abstractions.Parsers.Egauge;
using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Timeseries;

public record class EgaugeMeasured(EgaugeMeasurement measurement)
  : Event(measurement.timestamp);
