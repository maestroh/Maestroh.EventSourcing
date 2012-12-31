using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Maestroh.Command.Events;

namespace Maestroh.Command.EventStore
{
    public class EventAccess : IEventAccess
    {
        private readonly IFormatter _formatter;
        private const string ConnectionString = "server=localhost;user=root;database=eventstore;port=3306;password=12345;";

        public EventAccess(IFormatter formatter)
        {
            _formatter = formatter;
        }

        public void SaveEvent(IDomainEvent domainEvent)
        {
            var conn =new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            conn.Open();

            conn.Execute(@"insert event(AggregateId, Data, Version) values (@a, @b, @c)",
                new
                {
                    a = domainEvent.AggregateId,
                    b = Serialize(domainEvent),
                    c = domainEvent.Version
                });

            conn.Close();
        }

        public List<IDomainEvent> GetEventsForAggregate(Guid id)
        {
            var conn =new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            conn.Open();

            var result = conn.Query<RawEvent>("select * from event where AggregateId = @a", new { a = id });

            var converted =
                result.Select(
                    x =>
                    new Event()
                    {
                        AggregateId = x.AggregateId,
                        Data = Deserialize<IDomainEvent>(x.Data),
                        Version = x.Version
                    }).ToList();

            conn.Close();

            return converted.Select(x => x.Data).ToList();
        }

        private byte[] Serialize(object theObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                _formatter.Serialize(memoryStream, theObject);
                return memoryStream.ToArray();
            }
        }

        private TType Deserialize<TType>(object bytes)
        {
            using (var memoryStream = new MemoryStream((Byte[])bytes))
            {
                return (TType)_formatter.Deserialize(memoryStream);
            }
        }

        public int CurrentAggregateVersion(Guid id)
        {
            const string commandText = @"
                INSERT IGNORE INTO Aggregate VALUES (@aggregateId, 0);
                SELECT Version FROM Aggregate WHERE AggregateId = @aggregateId";

            var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            conn.Open();

            var result = conn.Query<int>(commandText, new { aggregateId = id }).FirstOrDefault();

            conn.Close();

            return result;
        }

        public void UpdateAggregateVersion(Guid id, int version)
        {
            var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            conn.Open();

            conn.Execute(@"update aggregate set version = @a where aggregateid=@b",
                new
                {
                    a = version,
                    b = id
                });

            conn.Close();
        }

        private class RawEvent
        {
            public Guid AggregateId { get; set; }
            public Byte[] Data { get; set; }
            public int Version { get; set; }

        }
    }
}
