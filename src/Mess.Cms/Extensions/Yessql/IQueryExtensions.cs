using System.Linq.Expressions;
using YesSql;
using YesSql.Indexes;

namespace Mess.Cms.Extensions.Yessql;

public static class IQueryExtensions
{
  public static IQuery<T, TIndex> In<T, TIndex, TMember>(
    this IQuery<T, TIndex> query,
    Expression<Func<TIndex, TMember>> expression,
    IEnumerable<TMember> values
  )
    where T : class
    where TIndex : IIndex
  {
    return expression is not MemberExpression memberExpression
      ? throw new ArgumentException("Expression must be a member expression")
      : query.Where(
        $"{memberExpression.Member.Name} IN ({string.Join(",", values.Select(value => $"'{value}'"))})"
      );
  }
}
