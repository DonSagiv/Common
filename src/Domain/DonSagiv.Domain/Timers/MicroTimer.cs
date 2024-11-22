namespace DonSagiv.Domain.Timers;

public sealed class MicroTimer : IDisposable
{
    #region Fields
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _timerTask;
    private long _interval;
    private long _ignoreEventIfLateBy = long.MaxValue;
    #endregion

    #region Delegates
    public event EventHandler<MicroTimerEventArgs>? MicroTimerElapsed;
    #endregion

    #region Properties
    public long Interval
    {
        get => Interlocked.Read(ref _interval);
        set => Interlocked.Exchange(ref _interval, value);
    }
    public long IgnoreEventIfLateBy
    {
        get => Interlocked.Read(ref _ignoreEventIfLateBy);
        set => Interlocked.Exchange(ref _ignoreEventIfLateBy, value);
    }
    public bool Enabled => _timerTask is { IsCompleted: false };
    #endregion

    #region Constructor
    public MicroTimer(long timerIntervalInMicroseconds = 0)
    {
        Interval = timerIntervalInMicroseconds;
    }
    #endregion

    #region Methods
    public void Start()
    {
        if (Enabled || Interval < 0)
        {
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        _timerTask = RunTimerAsync(_cancellationTokenSource.Token);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
    }

    public async Task StopAndWaitAsync(TimeSpan? timeout = null)
    {
        if (_timerTask is null)
        {
            return;
        }

        _cancellationTokenSource?.Cancel();

        try
        {
            await (_timerTask.WaitAsync(timeout ?? Timeout.InfiniteTimeSpan));
        }
        catch (OperationCanceledException)
        {
            // Expected when cancelling
        }
    }

    private async Task RunTimerAsync(CancellationToken cancellationToken)
    {
        var timerCount = 0;
        long nextNotification = 0;

        var microStopwatch = new MicroStopwatch();
        microStopwatch.Start();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var callbackFunctionExecutionTime = microStopwatch.ElapsedMicroseconds - nextNotification;
                var timerIntervalInMicroSecCurrent = Interlocked.Read(ref _interval);
                var ignoreEventIfLateByCurrent = Interlocked.Read(ref _ignoreEventIfLateBy);

                nextNotification += timerIntervalInMicroSecCurrent;
                timerCount++;

                long elapsedMicroseconds;

                while ((elapsedMicroseconds = microStopwatch.ElapsedMicroseconds) < nextNotification)
                {
                    await Task.Delay(0, cancellationToken);
                }

                var timerLateBy = elapsedMicroseconds - nextNotification;

                if (timerLateBy >= ignoreEventIfLateByCurrent)
                {
                    continue;
                }

                var timerEventArgs = new MicroTimerEventArgs(timerCount, elapsedMicroseconds, timerLateBy, callbackFunctionExecutionTime);

                MicroTimerElapsed?.Invoke(this, timerEventArgs);
            }
        }
        finally
        {
            microStopwatch.Stop();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
    #endregion
}