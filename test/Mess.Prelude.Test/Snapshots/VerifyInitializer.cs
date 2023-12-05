using DiffEngine;

namespace Mess.Prelude.Test.Snapshots;

public static class VerifyInitializer
{
  private static bool _initialized;
  private static readonly object _lock = new();

  public static void Initialize()
  {
    lock (_lock)
    {
      if (_initialized) return;

      VerifierSettings.UseStrictJson();
      VerifierSettings.SortJsonObjects();
      VerifierSettings.SortPropertiesAlphabetically();

      VerifierSettings.OnFirstVerify(
        (sourceFile, projectDirectory) =>
        {
          File.Copy(sourceFile.ReceivedPath, sourceFile.VerifiedPath, true);

          return Task.CompletedTask;
        }
      );

      DerivePathInfo(
        (sourceFile, projectDirectory, type, method) =>
        {
          var baseSnapshotDirectory = Path.Combine(
            projectDirectory,
            "Assets",
            "Snapshots"
          );

          return new PathInfo(
            baseSnapshotDirectory,
            type.Name,
            method.Name
          );
        }
      );

      ClipboardAccept.Enable();

      DiffTools.UseOrder(
        DiffTool.KDiff3,
        DiffTool.VisualStudioCode,
        DiffTool.VisualStudio
      );
      DiffRunner.MaxInstancesToLaunch(1);
      DiffRunner.Disabled = true;

      VerifyNewtonsoftJson.Initialize();

      _initialized = true;
    }
  }
}
