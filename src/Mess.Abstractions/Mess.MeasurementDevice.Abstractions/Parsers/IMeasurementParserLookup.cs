namespace Mess.MeasurementDevice.Abstractions.Parsers;

public interface IMeasurementParserLookup
{
  public IMeasurementParser? Get(string id);
}
