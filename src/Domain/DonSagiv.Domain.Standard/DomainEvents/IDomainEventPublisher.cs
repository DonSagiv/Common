using MediatR;

namespace DonSagiv.Domain.Standard.DomainEvents;

public interface IDomainEventPublisher
{
    void Publish(INotification notification);
}
