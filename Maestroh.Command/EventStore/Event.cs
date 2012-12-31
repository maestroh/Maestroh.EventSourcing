using System;
using Maestroh.Command.Events;

namespace Maestroh.Command.EventStore
{
    public class Event
    {
        public Guid AggregateId { get; set; }
        public IDomainEvent Data { get; set; }
        public int Version { get; set; }
    }
}