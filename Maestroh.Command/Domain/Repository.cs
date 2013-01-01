using System;
using Maestroh.Bus;
using Maestroh.Command.EventStore;

namespace Maestroh.Command.Domain
{
    public class Repository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _storage;
        private readonly IBus _bus;

        public Repository(IEventStore storage, IBus bus)
        {
            _storage = storage;
            _bus = bus;
        }

        public void Save(AggregateRoot aggregate, int expectedVersion)
        {
            var domainEvents = aggregate.GetUncommittedChanges();
            _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
            foreach (var domainEvent in domainEvents)
            {
                _bus.Publish(domainEvent);
            }
        }

        public T GetById(Guid id)
        {
            var obj = new T();
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadFromHistory(e);
            return obj;

        }
    }
}
