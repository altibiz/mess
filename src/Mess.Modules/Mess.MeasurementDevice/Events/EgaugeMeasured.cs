using Mess.MeasurementDevice.Abstractions.Parsers.Egauge;
using Mess.EventStore.Abstractions.Events;

namespace Mess.MeasurementDevice.Events;

public record class EgaugeMeasured(EgaugeMeasurement measurement)
  : Event(measurement.timestamp);
