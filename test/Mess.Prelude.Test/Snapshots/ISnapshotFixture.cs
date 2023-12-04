namespace Mess.System.Test.Snapshots;

public interface ISnapshotFixture
{
  public Task<string> MakeVerificationHash(params object?[] parameters);

  public Task Verify(object? @snapshot, string verificationHash);
}
