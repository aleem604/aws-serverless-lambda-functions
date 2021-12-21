using System;
using TinCore.Domain.Core.Events;

namespace TinCore.Domain.Events
{
    public class BusinessUpdatedEvent : Event
    {
        public BusinessUpdatedEvent(long id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.AggregateId = Guid.NewGuid();
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}