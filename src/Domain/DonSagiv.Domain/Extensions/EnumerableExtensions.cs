using DonSagiv.Domain.Delegates;

namespace DonSagiv.Domain.Extensions;

public static class EnumerableExtensions
{
    #region LINQ Extensions
    public static IEnumerable<TResult> SelectWhere<TResult, TSource>(this IEnumerable<TSource> input,
        SelectWhereDelegate<TResult, TSource> inputDelegate)
    {
        foreach(var item in input)
        {
            if(inputDelegate.Invoke(item, out var result))
            {
                yield return result;
            }
        }
    }
    #endregion
}
