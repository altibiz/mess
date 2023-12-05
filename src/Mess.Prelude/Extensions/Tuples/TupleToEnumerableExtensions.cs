using System.Runtime.CompilerServices;

namespace Mess.Prelude.Extensions.Tuples;

public static class TupleToEnumerableExtensions
{
  public static IEnumerable<object> ToEnumerable(this ITuple tuple)
  {
    var body = new List<object>();
    var hasTail = tuple.Length == 8;

    var bodyLength = hasTail ? tuple.Length - 1 : tuple.Length;
    for (var headIndex = 0; headIndex < bodyLength; headIndex++)
      body.Add(tuple[headIndex]!);

    if (hasTail)
    {
      var tail = (tuple[7] as ITuple)!;
      return body.Concat(tail.ToEnumerable());
    }

    return body;
  }
}
