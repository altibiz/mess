namespace Mess.Ozds.Abstractions.Timeseries;

public record Measurement(
  string Source,
  DateTimeOffset Timestamp,
  decimal ActiveEnergyImportTotal_Wh,
  decimal ActivePower_W
);
