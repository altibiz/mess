using Mess.EventStore.Abstractions.Parsers.Egauge;
using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Events;

public record class EgaugeMeasured(EgaugeMeasurement measurement)
  : Event(measurement.timestamp);
