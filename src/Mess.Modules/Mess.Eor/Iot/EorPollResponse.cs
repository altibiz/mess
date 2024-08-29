namespace Mess.Eor.Iot;

public record EorPollResponse(int Mode, bool Reset, bool Start, bool Stop, bool End = true);
