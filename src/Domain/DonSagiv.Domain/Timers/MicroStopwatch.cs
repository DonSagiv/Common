using System.Diagnostics;

namespace DonSagiv.Domain.Timers;

public class MicroStopwatch : Stopwatch
{
    #region Fields
    private readonly double _microSecPerTick = 1000000D / Frequency;
    #endregion

    #region Properties
    public long ElapsedMicroseconds => (long)(ElapsedTicks * _microSecPerTick);
    #endregion
}
