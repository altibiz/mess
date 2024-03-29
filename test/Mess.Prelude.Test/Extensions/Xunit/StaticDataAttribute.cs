using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

[AttributeUsage(
  AttributeTargets.Method,
  AllowMultiple = true)]
public class StaticDataAttribute : DataAttribute
{
  private readonly Type? _class;
  private readonly string _field;

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
    var @class = (_class ?? testMethod.DeclaringType) ??
                 throw new InvalidOperationException(
                   "Couldn't instantiate data");

    var field = @class.GetField(
      _field,
      BindingFlags.Static | BindingFlags.Public
    ) ?? throw new InvalidOperationException("Couldn't instantiate data");

    return field.GetValue(_class) switch
    {
      IEnumerable<object[]> data => data,
      object[] oneShot => new[] { oneShot },
      object oneArgument => new[] { new[] { oneArgument } },
      _ => throw new InvalidOperationException("Couldn't instantiate data")
    };
  }
}
