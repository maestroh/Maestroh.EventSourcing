using System;
using System.Runtime.Serialization.Formatters.Binary;
using Maestroh.Command.EventStore;
using NUnit.Framework;

namespace Maestroh.DataAccess.Tests
{
    class EventAccessTest
    {
        [Test]
        public void AggregateTest()
        {

            var eventAccess = new EventAccess(new BinaryFormatter());
            var id = Guid.NewGuid();

            Assert.IsTrue(eventAccess.CurrentAggregateVersion(id) == 0);

            eventAccess.UpdateAggregateVersion(id, 1);

            Assert.IsTrue(eventAccess.CurrentAggregateVersion(id) == 1);

        }
    }
}
