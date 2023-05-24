using DiffEngine;
using VerifyTests;
using VerifyXunit;

namespace Mess.Test.Snapshots;

public static class VerifyInitializer
{
  public static void Initialize()
  {
    lock (_lock)
    {
      if (_initialized)
      {
        return;
      }

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

      Verifier.DerivePathInfo(
        (sourceFile, projectDirectory, type, method) =>
        {
          var baseSnapshotDirectory = Path.Combine(
            projectDirectory,
            "Assets",
            "Snapshots"
          );
          // NOTE: sadly creates paths that are too long for Windows
          // var assemblyName = type.Assembly.GetName().Name!;
          // var namespaceName = type.Namespace!;
          // var subdirectories = namespaceName
          //   .TrimStart(assemblyName)
          //   .Trim(".")
          //   .Split(".");
          // var directory = baseSnapshotDirectory;
          // foreach (var subtiretrory in subdirectories)
          // {
          //   directory = Path.Combine(directory, subtiretrory);
          // }
          // directory = Path.Combine(directory, type.Name);

          return new(
            directory: baseSnapshotDirectory,
            typeName: type.Name, // NOTE: needed for some reason
            methodName: method.Name
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

  private static bool _initialized = false;
  private static readonly object _lock = new();
}
