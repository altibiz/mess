using System.Reflection;
using Mess.Cms.Extensions.Streams;

namespace Mess.Cms;

public static class EmbeddedResources
{
  public static T GetNewtonsoftJsonEmbeddedResource<T>(
    string name,
    Assembly? assemblyName = null
  )
  {
    return Prelude.EmbeddedResources
             .GetEmbeddedResource(name,
               assemblyName ?? Assembly.GetCallingAssembly())
             .FromNewtonsoftJsonStream<T>()
           ?? throw new InvalidOperationException(
             $"Resource {name} couldn't be parsed"
           );
  }

  public static object GetNewtonsoftJsonEmbeddedResource(
    string name,
    Type type,
    Assembly? assemblyName = null
  )
  {
    return Prelude.EmbeddedResources
             .GetEmbeddedResource(name,
               assemblyName ?? Assembly.GetCallingAssembly())
             .FromNewtonsoftJsonStream(type)
           ?? throw new InvalidOperationException(
             $"Resource {name} couldn't be parsed"
           );
  }
}
