using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Maestroh.Command.Events;

namespace Maestroh.Command.EventStore
{
    public class EventStore : IEventStore
    {
        //TODO: Transactions
        //TODO: Snapshotting
        //TODO: Publish Events

        private readonly IEventAccess _eventAccess;

        public EventStore(IEventAccess eventAccess)
        {
            _eventAccess = eventAccess;
        }

        public void SaveEvents(Guid id, List<IDomainEvent> events, int expectedVersion)
        {
            // check if expected version matches latest event version
            if (_eventAccess.CurrentAggregateVersion(id) != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;
            foreach (var e in events)
            {
                i++;
                _eventAccess.SaveEvent(e);
                // publish on the bus
            }

            _eventAccess.UpdateAggregateVersion(id, i);
        }
      
        public List<IDomainEvent> GetEventsForAggregate(Guid id)
        {
            return _eventAccess.GetEventsForAggregate(id);
        }

    }
}
