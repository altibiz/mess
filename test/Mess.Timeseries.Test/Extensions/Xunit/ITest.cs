using Xunit.Abstractions;

namespace Mess.Timeseries.Test.Extensions.Xunit;

public static class ITestExtensions
{
  public static string? GetTestIdentifier(this ITest? test) =>
    test?.DisplayName
      .RegexRemove(@"^Mess\.Timeseries\.Test\.")
      .RegexRemove(@"(?:Test)?(?:\(.*)?$")
      .RegexReplace(@"([a-z])([A-Z])", @"$1-$2")
      .ToLowerInvariant();
}
