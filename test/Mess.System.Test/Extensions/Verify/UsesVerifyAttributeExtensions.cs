using System.Reflection;
using VerifyXunit;

namespace Mess.System.Test.Extensions.Verify;

public static class UsesVerifyAttributeExtensions
{
  // NOTE: https://github.com/VerifyTests/Verify/blob/f5af3cd875ccf0bb55e2338ef9faffd5a6defc75/src/Verify.Xunit/UsesVerifyAttribute.cs#L7
  public static AsyncLocal<MethodInfo?> GetMethodStorage() =>
    typeof(UsesVerifyAttribute)
      .GetField("local", BindingFlags.NonPublic | BindingFlags.Static)
      ?.GetValue(null) as AsyncLocal<MethodInfo?>
    ?? throw new InvalidOperationException("Couldn't get method storage");
}
