using MediatR;

namespace DonSagiv.Domain.DomainEvents;

public interface IDomainEventPublisher
{
    void Publish(INotification notification);
}
