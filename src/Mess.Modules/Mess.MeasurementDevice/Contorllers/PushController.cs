using Microsoft.AspNetCore.Mvc;
using Mess.System.Extensions.Strings;
using Mess.MeasurementDevice.Abstractions.Parsers;
using Mess.MeasurementDevice.Abstractions.Storage;

namespace Mess.MeasurementDevice.Controllers;

public class PushController : Controller
{
  [HttpPost]
  [IgnoreAntiforgeryToken] // TODO: security
  public async Task<IActionResult> Index(
    [FromServices] IMeasurementParserLookup parserLookup,
    [FromServices] IMeasurementStorageStrategyLookup storageStragegyLookup,
    string parserId
  )
  {
    var parser = parserLookup.Get(parserId);
    if (parser is null)
    {
      return BadRequest($"Unknown parser for measurement");
    }

    var body = await Request.Body.EncodeAsync();
    var parsedMeasurement = await parser.ParseAsync(body);
    if (parsedMeasurement is null)
    {
      return BadRequest($"Failed parsing measurement");
    }

    var storageStrategy = storageStragegyLookup.Get(
      parsedMeasurement.Value.StorageStrategy
    );
    if (storageStrategy is null)
    {
      return BadRequest($"Unknown storage strategy for measurement");
    }

    var storeError = await storageStrategy.StoreAsync(parsedMeasurement.Value);
    if (storeError is not null)
    {
      return BadRequest($"Storage of measurement failed because {storeError}");
    }

    return Ok();
  }
}
