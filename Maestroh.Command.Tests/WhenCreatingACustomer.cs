using System;
using System.Collections.Generic;
using Maestroh.Bus;
using Maestroh.Command.CommandHandlers;
using Maestroh.Command.Commands;
using Maestroh.Command.Domain;
using Maestroh.Command.EventStore;
using Maestroh.Command.Events;
using Moq;
using NUnit.Framework;

namespace Maestroh.Command.Tests
{
    public class WhenCreatingACustomer
    {
        private Mock<IEventAccess> _eventAccess;
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _eventAccess = new Mock<IEventAccess>();
            _eventAccess.Setup(x => x.CurrentAggregateVersion(It.IsAny<Guid>())).Returns(0);
            _bus = new Mock<IBus>();

            var eventStore = new EventStore.EventStore(_eventAccess.Object);
            var repo = new Repository<Customer>(eventStore, _bus.Object);
            var handler = new CustomerCommandHandlers(repo);
            handler.Handle(new CreateCustomer("customer name"));
        }

        [Test]
        public void ThenTheRepositoryShouldSaveTheNewCustomer()
        {
            _eventAccess.Verify(x => x.SaveEvent(It.IsAny<CustomerCreatedEvent>()));
        }

        [Test]
        public void ThenTheRepositoryShouldPublishTheCustomerCreatedEvent()
        {
            _bus.Verify(x => x.Publish(It.IsAny<IDomainEvent>()), Times.Once());
        }
    }
}
