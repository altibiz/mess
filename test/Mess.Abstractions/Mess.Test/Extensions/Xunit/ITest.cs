using Xunit.Abstractions;
using Mess.System.Extensions.String;

namespace Mess.Test.Extensions.Xunit;

public static class ITestExtensions
{
  public static string GetTestIdentifier(this ITest test) =>
    test.DisplayName
      .RegexRemove(@"^Mess\.EventStore\.Test\.")
      .RegexRemove(@"(?:Test)?(?:\(.*)?$")
      .RegexReplace(@"([a-z])([A-Z])", @"$1-$2")
      .ToLowerInvariant();
}
