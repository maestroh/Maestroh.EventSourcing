using System;
using System.Collections.Generic;
using Maestroh.Command.Events;

namespace Maestroh.Command.EventStore
{
    public interface IEventAccess
    {
        void SaveEvent(IDomainEvent domainEvent);
        List<IDomainEvent> GetEventsForAggregate(Guid id);
        int CurrentAggregateVersion(Guid id);
        void UpdateAggregateVersion(Guid id, int version);
    }
}