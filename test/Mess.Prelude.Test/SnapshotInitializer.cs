using System.Runtime.CompilerServices;

#pragma warning disable CA1050
public class SnapshotInitializer
{
  [ModuleInitializer]
  public static void Initialize() => VerifyInitializer.Initialize();
}
#pragma warning restore CA1050
