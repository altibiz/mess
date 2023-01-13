using System.Reflection;
using System.Xml.Linq;
using Mess.System.Extensions.Object;

namespace Mess.System;

public static class Resources
{
  public static Stream GetEmbeddedResource(
    string name,
    Assembly? assembly = null
  )
  {
    assembly ??= Assembly.GetCallingAssembly();
    var fullName = $"{assembly.GetName().Name}.{name}";
    var stream = assembly.GetManifestResourceStream(fullName);

    if (stream is null)
    {
      throw new InvalidOperationException(
        $"Resource {fullName} does not exist"
      );
    }

    return stream;
  }

  public static T GetJsonEmbeddedResource<T>(
    string name,
    Assembly? assemblyName = null
  ) =>
    GetEmbeddedResource(name, assemblyName ?? Assembly.GetCallingAssembly())
      .FromJsonStream<T>()
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
