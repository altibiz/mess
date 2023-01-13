using System.Reflection;
using Xunit.Abstractions;

namespace Mess.Xunit.Extensions.Xunit;

public static class ITestOutputHelperExtensions
{
  // NOTE: https://github.com/xunit/xunit/issues/416#issuecomment-378512739
  public static ITest? GetTest(this ITestOutputHelper? output) =>
    (ITest?)
      output
        ?.GetType()
        .GetField("test", BindingFlags.Instance | BindingFlags.NonPublic)
        ?.GetValue(output);
}
