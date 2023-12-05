using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mess.Cms;
using Mess.Prelude.Extensions.Strings;
using Mess.Prelude.Extensions.Tuples;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
  AttributeTargets.Method,
  AllowMultiple = true)]
public class NewtonsoftJsonAssetDataAttribute : DataAttribute
{
  private readonly Assembly? _assembly;

  private readonly string? _id;

  public NewtonsoftJsonAssetDataAttribute()
  {
  }

  public NewtonsoftJsonAssetDataAttribute(string id)
  {
    _id = id;
  }

  public NewtonsoftJsonAssetDataAttribute(string id, Assembly assembly)
  {
    _id = id;
    _assembly = assembly;
  }

  public override IEnumerable<object[]> GetData(MethodInfo testMethod)
  {
    var assembly = _assembly ?? testMethod.DeclaringType?.Assembly;
    var assemblyName = assembly?.GetName().Name;
    if (assembly is null || assemblyName is null)
      throw new InvalidOperationException("Couldn't instantiate data");

    var parameters = testMethod.GetParameters();
    var tupleType = parameters.Length switch
    {
      7 => typeof(ValueTuple<,,,,,,>),
      6 => typeof(ValueTuple<,,,,,>),
      5 => typeof(ValueTuple<,,,,>),
      4 => typeof(ValueTuple<,,,>),
      3 => typeof(ValueTuple<,,>),
      2 => typeof(ValueTuple<,>),
      1 => typeof(ValueTuple<>),
      0 => typeof(ValueTuple),
      _ => throw new InvalidOperationException("Couldn't instantiate data")
    };
    var type = typeof(IEnumerable<>).MakeGenericType(
      tupleType.MakeGenericType(
        parameters.Select(parameter => parameter.ParameterType).ToArray()
      )
    );

    var id = _id;
    if (id is null)
    {
      var declaringType = testMethod.DeclaringType ??
                          throw new InvalidOperationException(
                            "Couldn't instantiate data");

      var @namespace = declaringType.Namespace ??
                       throw new InvalidOperationException(
                         "Couldn't instantiate data");

      var assetNamespace = @namespace.TrimStart(assemblyName).Trim(".");
      id = $"{assetNamespace}.{declaringType.Name}.{testMethod.Name}";
    }

    var resource = EmbeddedResources.GetNewtonsoftJsonEmbeddedResource(
      $"Assets.Resources.{id}.json",
      type,
      assembly
    );

    return resource is not IEnumerable castedResource
      ? throw new InvalidOperationException("Couldn't instantiate data")
      : castedResource
        .Cast<ITuple>()
        .Select(parameters => parameters.ToEnumerable().ToArray());
  }
}
