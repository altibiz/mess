using System.Reflection;
using Xunit.Abstractions;

namespace Mess.System.Test.Extensions.Xunit;

public static class ITestOutputHelperExtensions
{
  // NOTE: https://github.com/xunit/xunit/issues/416#issuecomment-378512739
  public static ITest GetTest(this ITestOutputHelper output)
  {
    var testField = output
      .GetType()
      .GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
    if (testField is null)
    {
      throw new InvalidOperationException("Couldn't get test field");
    }

    var test = (ITest?)testField.GetValue(output);
    if (test is null)
    {
      throw new InvalidOperationException("Couldn't get test");
    }

    return test;
  }
}
