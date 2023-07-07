namespace Mess.Eor.MeasurementDevice.Polling;

public record EorPollResponse(int Mode, bool Reset, bool Start, bool Stop);
