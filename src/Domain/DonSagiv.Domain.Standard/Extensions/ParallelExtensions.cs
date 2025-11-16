using DonSagiv.Domain.Standard.Enumerators;
using System;

namespace DonSagiv.Domain.Standard.Extensions;

public class ParallelExtensions
{
    public static int GetDegreesOfParalleism(ParallelType parallelType = ParallelType.UiThreadOpen)
    {
        var degreesOfParallelism = parallelType switch
        {
            ParallelType.Dual => 2,
            ParallelType.Half => Environment.ProcessorCount / 2,
            ParallelType.Full => Environment.ProcessorCount,
            ParallelType.UiThreadOpen => Environment.ProcessorCount - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(parallelType), parallelType, null)
        };

        // Occurs on a CPU without multiple cores or hyper-threading.
        // Minimum degrees of parallelism should be 1.
        if (degreesOfParallelism < 1)
        {
            degreesOfParallelism = 1;
        }

        return degreesOfParallelism;
    }
}