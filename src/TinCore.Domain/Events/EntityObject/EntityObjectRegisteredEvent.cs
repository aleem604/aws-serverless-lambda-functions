using System;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class EntityObjectRegisteredEvent : Event
    {
        public EntityObjectRegisteredEvent(int isn, short eo_type, short eo_subtype, string object_type)
        {
            this.isn = isn;
            this.eo_type = eo_type;
            this.eo_subtype = eo_subtype;
            this.AggregateId = Guid.NewGuid();
        }

        public int isn { get; set; }
        public short eo_type { get; set; }
        public short eo_subtype { get; set; }
        public string object_type { get; set; }
    }
}