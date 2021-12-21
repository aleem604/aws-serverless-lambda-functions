using System;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class BusinessRemovedEvent : Event
    {
        public BusinessRemovedEvent(long id)
        {
            this.Id = id;
            this.AggregateId = Guid.NewGuid();
        }

        public long Id { get; set; }

    }
}