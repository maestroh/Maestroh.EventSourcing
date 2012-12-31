using System;
using System.Collections.Generic;
using Maestroh.Command.Events;

namespace Maestroh.Command.Domain
{
    public class AggregateRoot
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _registeredEvents;
        private readonly List<IDomainEvent> _appliedEvents;

        public AggregateRoot()
        {
            _appliedEvents = new List<IDomainEvent>();
            _registeredEvents = new Dictionary<Type, Action<IDomainEvent>>();
        }

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        protected void RegisterEvent<TDomainEvent>(Action<TDomainEvent> eventHandler) where TDomainEvent: class, IDomainEvent
        {
            _registeredEvents.Add(typeof(TDomainEvent), theEvent => eventHandler(theEvent as TDomainEvent));
        }

        protected void Apply(IDomainEvent domainEvent)
        {
            Apply(domainEvent.GetType(), domainEvent);
            _appliedEvents.Add(domainEvent);
        }

        private void Apply(Type type, IDomainEvent domainEvent)
        {
            Action<IDomainEvent> handler;

            if (!_registeredEvents.TryGetValue(type, out handler))
                throw new UnregisteredDomainEventException(string.Format("The requested domain event '{0}' is not registered in '{1}'", type.FullName, GetType().FullName));

            handler(domainEvent);
        }

        public List<IDomainEvent> GetUncommittedChanges()
        {
            return _appliedEvents;
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> events)
        {
            foreach (var e in events) Apply(e.GetType(), e);
        }
    }
}
