using Xunit.Abstractions;
using Mess.Prelude.Extensions.Strings;

namespace Mess.Prelude.Test.Extensions.Xunit;

public static class ITestExtensions
{
  public static string GetTestIdentifier(this ITest test) =>
    test.DisplayName
      .RegexRemove(@"(?:Test)?(?:\(.*)?$")
      .RegexReplace(@"([a-z])([A-Z])", @"$1-$2")
      .ToLowerInvariant();
}
