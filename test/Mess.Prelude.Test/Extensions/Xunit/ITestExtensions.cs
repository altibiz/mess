using Mess.Prelude.Extensions.Strings;
using Xunit.Abstractions;

namespace Mess.Prelude.Test.Extensions.Xunit;

public static class ITestExtensions
{
  public static string GetTestIdentifier(this ITest test)
  {
    return test.DisplayName
      .RegexRemove(@"(?:Test)?(?:\(.*)?$")
      .RegexReplace(@"([a-z])([A-Z])", @"$1-$2")
      .ToLowerInvariant();
  }
}
