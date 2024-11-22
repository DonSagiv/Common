namespace DonSagiv.Domain.Timers;

public class MicroTimerEventArgs : EventArgs
{
    #region Properties
    public int TimerCount { get; }
    public long ElapsedMicroseconds { get; }
    public long TimerLateBy { get; }
    public long CallbackFunctionExecutionTime { get; }
    #endregion
    
    #region Constructors
    public MicroTimerEventArgs(int timerCount,
        long elapsedMicroseconds,
        long timerLateBy,
        long callbackFunctionExecutionTime)
    {
        TimerCount = timerCount;
        ElapsedMicroseconds = elapsedMicroseconds;
        TimerLateBy = timerLateBy;
        CallbackFunctionExecutionTime = callbackFunctionExecutionTime;
    }
    #endregion
}