using System.Reflection;
using Mess.System.Extensions.Objects;
using Mess.Test.Extensions.Verify;
using Mess.Test.Extensions.Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace Mess.Test.Snapshots;

public class SnapshotFixture : ISnapshotFixture, IDisposable, IAsyncDisposable
{
  public Task<string> MakeVerificationHash(params object?[]? parameters)
  {
    var verificationHash = MakeParameterHash(parameters);
    return Task.FromResult(verificationHash);
  }

  public async Task Verify(object? @object, string verificationHash)
  {
    await Verifier.Verify(@object).UseTextForParameters(verificationHash);
  }

  void IDisposable.Dispose()
  {
    _local.Value = null;
  }

  ValueTask IAsyncDisposable.DisposeAsync()
  {
    _local.Value = null;
    return ValueTask.CompletedTask;
  }

  private string MakeParameterHash(params object?[]? parameters)
  {
    if (parameters is null or { Length: 0 })
    {
      return "empty-parameters";
    }
    else
    {
      return parameters
        .Select(
          argument =>
            new { Type = argument?.GetType()?.FullName, Argument = argument }
        )
        .GetJsonMurMurHash();
    }
  }

  public SnapshotFixture(
    IServiceProvider serviceProvider,
    ITestOutputHelperAccessor testOutputHelperAccessor
  )
  {
    _testOutputHelper = testOutputHelperAccessor.Output!;
    _test = _testOutputHelper.GetTest();
    _local = UsesVerifyAttributeExtensions.GetMethodStorage();
    _local.Value = _test.TestCase.TestMethod.Method.ToRuntimeMethod();
  }

  private readonly ITestOutputHelper _testOutputHelper;
  private readonly ITest _test;
  private readonly AsyncLocal<MethodInfo?> _local;

  // TODO: use when we can get to the parameters here
  // private string GetParameterHash()
  // {
  //   return MakeParameterHash(_test.TestCase.TestMethodArguments);
  // }
}
