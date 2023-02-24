using Microsoft.Extensions.DependencyInjection;
using Mess.MeasurementDevice.Abstractions.Parsers;

namespace Mess.MeasurementDevice.Parsers;

public class MeasurementParserLookup : IMeasurementParserLookup
{
  public IMeasurementParser? Get(string id) =>
    _services
      .GetServices<IMeasurementParser>()
      .FirstOrDefault(storageStrategy => storageStrategy.Id == id);

  public MeasurementParserLookup(IServiceProvider services)
  {
    _services = services;
  }

  private readonly IServiceProvider _services;
}
