using System;
using Maestroh.Command.Events;

namespace Maestroh.Command.Domain
{
    public class Customer : AggregateRoot
    {
        private Customer(string customerName)
        {
            RegisterEvents();

            Apply(new CustomerCreatedEvent(Guid.NewGuid(), customerName));
        }

        public static Customer CreateNew(string customerName)
        {
            return new Customer(customerName);
        }

        private void RegisterEvents()
        {
            RegisterEvent<CustomerCreatedEvent>(OnCustomerCreated);
        }

        private void OnCustomerCreated(CustomerCreatedEvent customerCreatedEvent)
        {
            Id = customerCreatedEvent.AggregateId;
        }
    }
}
