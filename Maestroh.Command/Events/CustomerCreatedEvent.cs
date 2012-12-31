using System;

namespace Maestroh.Command.Events
{
    [Serializable]
    public class CustomerCreatedEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string CustomerName { get; set; }

        public CustomerCreatedEvent(Guid customerId, string customerName)
        {
            AggregateId = customerId;
            CustomerName = customerName;
        }

       
    }
}
