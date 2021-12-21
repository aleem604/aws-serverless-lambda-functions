using System;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class LocationRegisteredEvent : Event
    {
        public LocationRegisteredEvent(long LocationId, eLocationType locationType)
        {
            this.LocationId = LocationId;
            this.LocationType = locationType;
            this.AggregateId = Guid.NewGuid();
        }

        public long LocationId { get; set; }
        public eLocationType LocationType { get; set; }
    }
}