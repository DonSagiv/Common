namespace DonSagiv.Domain.Delegates;

public delegate bool SelectWhereDelegate<TResult, in TSource>(TSource source, out TResult result);

