using System;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class LocationUpdatedEvent : Event
    {
        public LocationUpdatedEvent(long locationId, eLocationType location_Type)
        {
            this.LocationId = locationId;
            this.Location_Type = location_Type;
            this.AggregateId = Guid.NewGuid();
        }

        public long LocationId { get; set; }
        public eLocationType Location_Type { get; set; }
    }
}