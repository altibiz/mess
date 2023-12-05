using System.Reflection;
using Mess.Prelude.Extensions.Objects;
using Mess.Prelude.Test.Extensions.Verify;
using Mess.Prelude.Test.Extensions.Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace Mess.Prelude.Test.Snapshots;

public sealed class SnapshotFixture : ISnapshotFixture, IDisposable,
  IAsyncDisposable
{
  private readonly AsyncLocal<MethodInfo?> _local;

  private readonly Func<object?[]?, string> _makeParameterHash;
  private readonly ITest _test;

  private readonly ITestOutputHelper _testOutputHelper;

  public SnapshotFixture(
    IServiceProvider serviceProvider,
    Func<object?[]?, string> makeParameterHash
  )
  {
    _testOutputHelper = serviceProvider
      .GetRequiredService<ITestOutputHelperAccessor>()
      .Output!;
    _test = _testOutputHelper.GetTest();
    _local = UsesVerifyAttributeExtensions.GetMethodStorage();
    _local.Value = _test.TestCase.TestMethod.Method.ToRuntimeMethod();
    _makeParameterHash = makeParameterHash;
  }

  ValueTask IAsyncDisposable.DisposeAsync()
  {
    _local.Value = null;
    return ValueTask.CompletedTask;
  }

  void IDisposable.Dispose()
  {
    _local.Value = null;
  }

  public Task<string> MakeVerificationHash(params object?[]? parameters)
  {
    var verificationHash = _makeParameterHash(parameters);
    return Task.FromResult(verificationHash);
  }

  public async Task Verify(object? snapshot, string verificationHash)
  {
    await Verifier.Verify(snapshot).UseTextForParameters(verificationHash);
  }

  private static string MakeParameterHash(params object?[]? parameters)
  {
    return parameters is null or { Length: 0 }
      ? "empty-parameters"
      : parameters
        .Select(
          argument =>
            new { Type = argument?.GetType()?.FullName, Argument = argument }
        )
        .GetJsonMurMurHash();
  }
}
