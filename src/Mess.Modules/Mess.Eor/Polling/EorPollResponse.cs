namespace Mess.Eor.Polling;

public record EorPollResponse(int Mode, bool Reset, bool Start, bool Stop);
