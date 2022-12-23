using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
  AttributeTargets.Method,
  AllowMultiple = true,
  Inherited = true
)]
public class StaticDataAttribute : DataAttribute
{
  public StaticDataAttribute(Type @class, string field)
  {
    Class = @class;
    Field = field;
  }

  public Type Class { get; init; }
  public string Field { get; init; }

  public override IEnumerable<object[]> GetData(MethodInfo testMethod)
  {
    var field = Class.GetField(
      Field,
      BindingFlags.Static | BindingFlags.Public
    );
    if (field is null)
    {
      throw new InvalidOperationException("Couldn't instantiate data");
    }

    return field.GetValue(Class) switch
    {
      IEnumerable<object[]> data => data,
      object[] oneShot => new[] { oneShot },
      object oneArgument => new[] { new[] { oneArgument } },
      _ => throw new InvalidOperationException("Couldn't instantiate data")
    };
  }
}
