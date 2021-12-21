using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Commands;
using TinCore.Domain.Core.Notifications;
using TinCore.Domain.Interfaces;
using MediatR;
using System;

namespace TinCore.Domain.CommandHandlers
{
    public class CommandHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;

        public CommandHandler(IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            _uow = uow;
            _notifications = (DomainNotificationHandler)notifications;
            _bus = bus;
        }

        protected void NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        public bool Commit()
        {
            if (_notifications.HasNotifications()) return false;
            try
            {
                return _uow.Commit();
            }
            catch (Exception ex)
            {
                _bus.RaiseEvent(new DomainNotification("Commit", $"We had a problem during saving your data. {ex.Message}"));
                return false;
            }            
        }
    }
}