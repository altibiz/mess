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
    _class = @class;
    _field = field;
  }

  public StaticDataAttribute(string field)
  {
    _field = field;
  }

  public override IEnumerable<object[]> GetData(MethodInfo testMethod)
  {
    var @class = _class ?? testMethod.DeclaringType;
    if (@class is null)
    {
      throw new InvalidOperationException("Couldn't instantiate data");
    }

    var field = @class.GetField(
      _field,
      BindingFlags.Static | BindingFlags.Public
    );
    if (field is null)
    {
      throw new InvalidOperationException("Couldn't instantiate data");
    }

    return field.GetValue(_class) switch
    {
      IEnumerable<object[]> data => data,
      object[] oneShot => new[] { oneShot },
      object oneArgument => new[] { new[] { oneArgument } },
      _ => throw new InvalidOperationException("Couldn't instantiate data")
    };
  }

  private Type? _class;
  private string _field;
}
