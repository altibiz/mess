using Xunit;

namespace Mess.Test.Extensions.Xunit;

public static class AssertExtensions
{
  public static void ElementsEqual<T>(
    IEnumerable<T> expected,
    IEnumerable<T> actual
  )
  {
    Assert.True(expected.ToHashSet().SetEquals(actual.ToHashSet()));
  }

  public static void Subset<T>(IEnumerable<T> expected, IEnumerable<T> actual)
  {
    Assert.Subset(expected.ToHashSet(), actual.ToHashSet());
  }

  public static void Superset<T>(IEnumerable<T> expected, IEnumerable<T> actual)
  {
    Assert.Superset(expected.ToHashSet(), actual.ToHashSet());
  }

  public static void ProperSubset<T>(
    IEnumerable<T> expected,
    IEnumerable<T> actual
  )
  {
    Assert.ProperSubset(expected.ToHashSet(), actual.ToHashSet());
  }

  public static void ProperSuperset<T>(
    IEnumerable<T> expected,
    IEnumerable<T> actual
  )
  {
    Assert.ProperSuperset(expected.ToHashSet(), actual.ToHashSet());
  }

  public static void Unique<T>(IEnumerable<T> actual)
  {
    Assert.True(actual.ToHashSet().Count == actual.ToList().Count);
  }

  public static void OneOf<T>(T actual, IEnumerable<T> expected)
  {
    Assert.Contains(actual, expected);
  }
}
