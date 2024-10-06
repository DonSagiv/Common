using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonSagiv.Domain.Extensions;

public static class QueueExtensions
{
    public static bool TryDequeue<T>(this Queue<T> targetQueue, out T? value)
    {
        using var enumerator = targetQueue.GetEnumerator();

        if (enumerator.MoveNext())
        {
            value = targetQueue.Dequeue();

            return true;
        }

        value = default;

        return false;
    }
}
