namespace DonSagiv.Domain.Enumerators;

public enum ParallelType
{
    /// <summary>
    /// Use Only 2 threads (if available)
    /// </summary>
    Dual,
    /// <summary>
    /// Use half the available threads.
    /// </summary>
    Half,
    /// <summary>
    /// Use all threads except UI thread.
    /// </summary>
    UiThreadOpen,
    /// <summary>
    /// Use all available threads (caution, may cause UI issues).
    /// </summary>
    Full,
}