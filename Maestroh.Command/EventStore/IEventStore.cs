using System;
using System.Collections.Generic;
using Maestroh.Command.Events;

namespace Maestroh.Command.EventStore
{
    public interface IEventStore
    {
        void SaveEvents(Guid id, List<IDomainEvent> events, int expectedVersion);
        List<IDomainEvent> GetEventsForAggregate(Guid id);
    }
}
