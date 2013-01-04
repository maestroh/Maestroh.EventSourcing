using Dapper;
using Maestroh.Command.Events;

namespace Maestroh.Query.EventHandlers
{
    class CustomerEventHandlers
    {
        private const string ConnectionString = "server=localhost;user=root;database=eventstore;port=3306;password=12345;";

        public static void Handle(CustomerCreatedEvent customerCreatedEvent)
        {
            var conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            conn.Open();

            conn.Execute(@"insert customers(CustomerID, Name) values (@a, @b)",
                new
                {
                    a = customerCreatedEvent.AggregateId,
                    b = customerCreatedEvent.CustomerName
                });

            conn.Close();
            
        }
    }
}
