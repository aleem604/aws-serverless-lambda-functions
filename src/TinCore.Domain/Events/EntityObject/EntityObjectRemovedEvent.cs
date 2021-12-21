using System;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class EntityObjectRemovedEvent : Event
    {
        public EntityObjectRemovedEvent(string isn)
        {
            this.isn = isn;
            this.AggregateId = Guid.NewGuid();
        }

        public string isn { get; set; }

    }
}