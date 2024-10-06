namespace DonSagiv.Domain.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> targetCollection, IEnumerable<T> itemsToAdd)
    {
        foreach (var item in itemsToAdd)
        {
            targetCollection.Add(item);
        }
    }

    public static void RemoveRange<T>(this ICollection<T> targetCollection, IEnumerable<T> itemsToRemove)
    {
        foreach(var item in itemsToRemove)
        {
            targetCollection.Remove(item);
        }
    }

    public static void AddRange<T>(this ICollection<T> targetCollection, params T[] itemsToAdd)
    {
        targetCollection.AddRange(itemsToAdd.AsEnumerable());
    }

    public static void RemoveRange<T>(this ICollection<T> targetCollection, params T[] itemsToRemove)
    {
        targetCollection.RemoveRange(itemsToRemove.AsEnumerable());
    }

    public static void Append<T>(this ICollection<T> targetCollection, T itemToAppend)
    {
        if (targetCollection.Contains(itemToAppend))
        {
            return;
        }

        targetCollection.Add(itemToAppend);
    }
}
