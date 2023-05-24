namespace Mess.Test.Snapshots;

public interface ISnapshotFixture
{
  public Task<string> MakeVerificationHash(params object?[] parameters);

  public Task Verify(object? @object, string verificationHash);
}
