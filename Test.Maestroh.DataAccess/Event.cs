using System;
using Maestroh.Command.Events;

namespace Maestroh.DataAccess.Tests
{
    public class Event
    {
        public Guid AggregateId { get; set; }
        public IDomainEvent Data { get; set; }
        public int Version { get; set; }
    }
}