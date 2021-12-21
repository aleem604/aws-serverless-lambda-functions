using System;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class LocationRemovedEvent : Event
    {
        public LocationRemovedEvent(long LocationId)
        {
            this.LocationId = LocationId;
            this.AggregateId = Guid.NewGuid();
        }

        public long LocationId { get; set; }

    }
}