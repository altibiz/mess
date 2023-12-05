using System.Reflection;

namespace Mess.Prelude.Test.Extensions.Verify;

public static class UsesVerifyAttributeExtensions
{
  // NOTE: https://github.com/VerifyTests/Verify/blob/f5af3cd875ccf0bb55e2338ef9faffd5a6defc75/src/Verify.Xunit/UsesVerifyAttribute.cs#L7
  public static AsyncLocal<MethodInfo?> GetMethodStorage()
  {
    return typeof(UsesVerifyAttribute)
             .GetField("local", BindingFlags.NonPublic | BindingFlags.Static)
             ?.GetValue(null) as AsyncLocal<MethodInfo?>
           ?? throw new InvalidOperationException(
             "Couldn't get method storage");
  }
}
