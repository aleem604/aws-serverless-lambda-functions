using System;
using System.Threading;
using System.Threading.Tasks;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using TinCore.Domain.Events;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using MediatR;

namespace TinCore.Domain.CommandHandlers
{
    public class LocationCommandHandler : CommandHandler,
        IRequestHandler<NewLocationCommand, bool>,
        IRequestHandler<UpdateLocationCommand, bool>,
        IRequestHandler<RemoveLocationCommand, bool>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMediatorHandler Bus;

        public LocationCommandHandler(ILocationRepository geoCitiesRepository, 
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) :base(uow, bus, notifications)
        {
            _locationRepository = geoCitiesRepository;
            Bus = bus;
        }

        public Task<bool> Handle(NewLocationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var location = new Location(message.LocationId, message.LocationType);

            _locationRepository.Add(location);

            if (Commit())
            {
                Bus.RaiseEvent(new LocationRegisteredEvent(location.Id, location.LocationType));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateLocationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var location = new Location(message.LocationId, message.LocationType);
            var existingLocation = _locationRepository.GetById(location.Id);

            if (existingLocation != null && existingLocation.Id != location.Id)
            {
                if (!existingLocation.Equals(location))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType,"The customer e-mail has already been taken."));
                    return Task.FromResult(false);
                }
            }

            _locationRepository.Update(location);

            if (Commit())
            {
                Bus.RaiseEvent(new LocationUpdatedEvent(location.Id, location.LocationType));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveLocationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _locationRepository.Remove(message.LocationId);

            if (Commit())
            {
                Bus.RaiseEvent(new LocationRemovedEvent(message.LocationId));
            }

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _locationRepository.Dispose();
        }
    }
}