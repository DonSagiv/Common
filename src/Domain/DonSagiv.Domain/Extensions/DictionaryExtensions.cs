namespace DonSagiv.Domain.Extensions;

public static class DictionaryExtensions
{
    public static void Append<TKey, TValue>(this IDictionary<TKey, TValue> targetDictionary, TKey key, TValue value)
    {
        if (targetDictionary.ContainsKey(key))
        {
            return;
        }

        targetDictionary[key] = value; 
    }
}
