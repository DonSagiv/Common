using System.Diagnostics.CodeAnalysis;

namespace DonSagiv.Domain.Comparers;

public class EqualityComparer<T> : IEqualityComparer<T>
    where T : IEquatable<T>
{
    #region Methods
    public virtual bool Equals(T? x, T? y)
    {
        return x?.Equals(y) ?? false;
    }

    public int GetHashCode([DisallowNull] T obj)
    {
        return obj.GetHashCode();
    }
    #endregion
}