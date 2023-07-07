using Mess.System.Test.Extensions.Xunit;
using Xunit.DependencyInjection;

namespace Mess.System.Test.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static string GetTestId(this IServiceProvider serviceProvider)
  {
    var testOutputHelper = serviceProvider
      .GetRequiredService<ITestOutputHelperAccessor>()
      .Output;
    if (testOutputHelper is null)
    {
      throw new InvalidOperationException(
        "ITestOutputHelperAccessor.Output is null."
      );
    }

    var test = testOutputHelper.GetTest();

    var testId = test.GetTestIdentifier();

    return testId;
  }
}
