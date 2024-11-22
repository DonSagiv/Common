using System.Reactive;

namespace DonSagiv.Domain.Timers;
public class MicroTimerObservable(long intervalMicroSeconds) : ObservableBase<long>
{
    #region Fields
    private readonly MicroTimer _microTimer = new(intervalMicroSeconds);
    #endregion

    #region Static Methods
    public static MicroTimerObservable FrequencyHz(double frequencyHz)
    {
        var periodMicroSeconds = Convert.ToInt64(1E6 / frequencyHz);

        return new MicroTimerObservable(periodMicroSeconds);
    }

    public static MicroTimerObservable TimeSpan(TimeSpan timeSpanInput)
    {
        var periodMicroSeconds = Convert.ToInt64(timeSpanInput.TotalMicroseconds * 1000);

        return new MicroTimerObservable(periodMicroSeconds);
    }
    #endregion

    #region Methods
    protected override IDisposable SubscribeCore(IObserver<long> observer)
    {
        _microTimer.MicroTimerElapsed += (s, e) => observer.OnNext(e.ElapsedMicroseconds);

        _microTimer.Start();

        return System.Reactive.Disposables.Disposable.Create(() => _microTimer.Stop());
    }
    #endregion
}
