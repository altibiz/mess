using System.Runtime.CompilerServices;

public class SnapshotInitializer
{
  [ModuleInitializer]
  public static void Initialize() => VerifyInitializer.Initialize();
}
