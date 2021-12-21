using System.Threading;
using System.Threading.Tasks;
using TinCore.Domain.Events;
using MediatR;

namespace TinCore.Domain.EventHandlers
{
    public class LocationEventHandler :
        INotificationHandler<LocationRegisteredEvent>,
        INotificationHandler<LocationUpdatedEvent>,
        INotificationHandler<LocationRemovedEvent>
    {
        public Task Handle(LocationUpdatedEvent message, CancellationToken cancellationToken)
        {
            // Send some notification e-mail

            return Task.CompletedTask;
        }

        public Task Handle(LocationRegisteredEvent message, CancellationToken cancellationToken)
        {
            // Send some greetings e-mail

            return Task.CompletedTask;
        }

        public Task Handle(LocationRemovedEvent message, CancellationToken cancellationToken)
        {
            // Send some see you soon e-mail

            return Task.CompletedTask;
        }
    }
}