using Mess.Prelude.Test.Extensions.Xunit;
using Xunit.DependencyInjection;

namespace Mess.Prelude.Test.Extensions.Microsoft;

public static class IServiceProviderExtensions
{
  public static string GetTestId(this IServiceProvider serviceProvider)
  {
    var testOutputHelper = serviceProvider
      .GetRequiredService<ITestOutputHelperAccessor>()
      .Output ?? throw new InvalidOperationException(
      "ITestOutputHelperAccessor.Output is null."
    );

    var test = testOutputHelper.GetTest();

    var testId = test.GetTestIdentifier();

    return testId;
  }
}
