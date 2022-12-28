using Mess.EventStore.Parsers.Egauge;

namespace Mess.EventStore.Events;

public record class EgaugeMeasured(EgaugeMeasurement measurement) { }
