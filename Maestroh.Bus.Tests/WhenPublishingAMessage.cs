using System;
using System.Threading;
using Maestroh.Command.Commands;
using Maestroh.Command.Events;
using Moq;
using NUnit.Framework;

namespace Maestroh.Bus.Tests
{
    public class WhenPublishingAMessage
    {
        [Test]
        public void TheHandlerShouldProcessTheMessage()
        {
            var stub = new HandlerStub();
            var bus = new LocalBus();
            bus.RegisterHandler<CreateCustomer>(stub.Handle);
            var command = new CreateCustomer(Guid.NewGuid(), "customer name");
            bus.Publish(command);
            Assert.IsTrue(stub.ResetEvent.WaitOne(10000));

        }

        private class HandlerStub
        {
            public AutoResetEvent ResetEvent = new AutoResetEvent(false);

            public void Handle(CreateCustomer message)
            {
                ResetEvent.Set();
            }
        }
    }
}
