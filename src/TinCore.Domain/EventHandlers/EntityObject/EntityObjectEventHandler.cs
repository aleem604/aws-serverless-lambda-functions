using System.Threading;
using System.Threading.Tasks;
using TinCore.Domain.Events;
using MediatR;

namespace TinCore.Domain.EventHandlers
{
    public class EntityObjectEventHandler :
        INotificationHandler<EntityObjectRegisteredEvent>,
        INotificationHandler<EntityObjectUpdatedEvent>,
        INotificationHandler<EntityObjectRemovedEvent>
    {
        public Task Handle(EntityObjectUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(EntityObjectRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(EntityObjectRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}