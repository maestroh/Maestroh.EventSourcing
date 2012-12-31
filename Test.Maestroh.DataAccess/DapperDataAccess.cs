using System;
using System.Linq;
using Dapper;
using Maestroh.Command.Events;
using NUnit.Framework;

namespace Maestroh.DataAccess.Tests
{
    public class DapperDataAccess
    {
        [Test]
        public void CrudTest()
        {
            // insert
            var conn =
                new MySql.Data.MySqlClient.MySqlConnection(
                    "server=localhost;user=root;database=eventstore;port=3306;password=12345;");
            conn.Open();

            var domainEvent = new CustomerCreatedEvent(Guid.NewGuid(), "maestroh") {Version = 1};

            conn.Execute(@"insert event(AggregateId, Data, Version) values (@a, @b, @c)", new { a = domainEvent.AggregateId, b = Formatter.Serialize(domainEvent), c = domainEvent.Version });

            // read
            var result = conn.Query<RawEvent>("select * from event");
            Assert.IsNotNull(result);

            var converted =
                result.Select(
                    x =>
                    new Event()
                        {
                            AggregateId = x.AggregateId,
                            Data = Formatter.Deserialize<IDomainEvent>(x.Data),
                            Version = x.Version
                        }).ToList();

            Assert.IsNotNull(converted);

            conn.Execute(@"delete from event");

            conn.Close();

        }
    }
}
