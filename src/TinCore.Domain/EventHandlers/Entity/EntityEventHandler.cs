using System.Threading;
using System.Threading.Tasks;
using TinCore.Domain.Events;
using MediatR;

namespace TinCore.Domain.EventHandlers
{
    public class EntityEventHandler :
        INotificationHandler<BusinessRegisteredEvent>,
        INotificationHandler<BusinessUpdatedEvent>,
        INotificationHandler<BusinessRemovedEvent>
    {

        public Task Handle(BusinessRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }
        public Task Handle(BusinessUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(BusinessRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}