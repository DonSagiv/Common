using DonSagiv.Domain.DependencyInjection;
using MediatR;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Unit = System.Reactive.Unit;

namespace DonSagiv.Domain.DomainEvents;

[Export(typeof(IDomainEventPublisher), creationPolicy: CreationPolicy.Scoped)]
internal class DomainEventPublisher : IDomainEventPublisher
{
    #region Fields
    private readonly object _syncObject = new();
    private readonly Subject<Unit> _domainEventEnqueueSubject = new();
    private readonly IPublisher _publisher;
    private readonly ConcurrentQueue<INotification> _domainEventQueue;

    private bool _busy;
    #endregion

    #region Constructor
    public DomainEventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
        _domainEventQueue = new ConcurrentQueue<INotification>();

        _domainEventEnqueueSubject
            .Select(StartPublish)
            .SelectMany(PublishDomainEventsAsync)
            .Subscribe(EndPublish);
    }
    #endregion

    #region Methods
    public void Publish(INotification notification)
    {
        _domainEventQueue.Enqueue(notification);

        if (IsBusy())
        {
            return;
        }

        _domainEventEnqueueSubject.OnNext(Unit.Default);
    }

    private T StartPublish<T>(T input)
    {
        lock (_syncObject)
        {
            _busy = true;
        }

        return input;
    }

    private async Task<T> PublishDomainEventsAsync<T>(T input)
    {
        while(_domainEventQueue.TryDequeue(out var domainEvent))
        {
            await _publisher.Publish(domainEvent);
        }

        return input;
    }

    private bool IsBusy()
    {
        lock (_syncObject)
        {
            return _busy;
        }
    }

    private void EndPublish<T>(T input)
    {
        lock (_syncObject)
        {
            _busy = false;
        }
    }
    #endregion
}
