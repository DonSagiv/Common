using DonSagiv.Domain.Delegates;

namespace DonSagiv.Domain.Extensions;

public static class EnumerableExtensions
{
    #region Custom LINQ Extensions
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

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> targetEnumerable, params T[] itemsToConcat)
    {
        return targetEnumerable.Concat(itemsToConcat);
    }

    public static T? OnlyOrDefault<T>(this IEnumerable<T> targetEnumerable)
    {
        var seenValues = new HashSet<T>();

        T? returnValue = default;

        foreach (var item in targetEnumerable)
        {
            if (!seenValues.Add(item))
            {
                continue;
            }

            if(seenValues.Count > 1)
            {
                return default;
            }

            returnValue = item;
        }

        return returnValue;
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> targetEnumerable)
    {
        var random = new Random();

        return targetEnumerable.OrderBy(x => random.Next());
    }
    #endregion
}
