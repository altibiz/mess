using System.Reflection;
using System.Xml.Linq;
using Mess.Prelude.Extensions.Streams;
using Mess.Prelude.Extensions.Strings;

namespace Mess.Prelude;

public static partial class EmbeddedResources
{
  public static Stream GetEmbeddedResource(
    string name,
    Assembly? assembly = null
  )
  {
    assembly ??= Assembly.GetCallingAssembly();
    var fullName = $"{assembly.GetName().Name}.{name}";

    var stream = assembly.GetManifestResourceStream(fullName) ?? throw new InvalidOperationException(
        $"Resource {fullName} does not exist. "
          + $"Here are the available resources for the given assembly '{assembly.GetName().Name}':\n"
          + string.Join("\n", assembly.GetManifestResourceNames())
      );

    return stream;
  }

  public static string GetStringEmbeddedResource(
    string name,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .Encode();

  public static T GetJsonEmbeddedResource<T>(
    string name,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .FromJsonStream<T>()
    ?? throw new InvalidOperationException(
      $"Resource {name} couldn't be parsed"
    );

  public static object GetJsonEmbeddedResource(
    string name,
    Type type,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .FromJsonStream(type)
    ?? throw new InvalidOperationException(
      $"Resource {name} couldn't be parsed"
    );

  public static T GetXmlEmbeddedResource<T>(
    string name,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .FromXmlStream<T>()
    ?? throw new InvalidOperationException(
      $"Resource {name} couldn't be parsed"
    );

  public static XDocument GetXmlEmbeddedResource(
    string name,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .FromXmlStream()
    ?? throw new InvalidOperationException(
      $"Resource {name} couldn't be parsed"
    );
}
