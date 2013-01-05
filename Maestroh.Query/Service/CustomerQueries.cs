using System.Collections.Generic;
using Dapper;
using Maestroh.Bus;
using Maestroh.Command.Events;
using Maestroh.Query.DTO;
using Maestroh.Query.EventHandlers;

namespace Maestroh.Query.Service
{
    public class CustomerQueries
    {
        private readonly IBus _bus;

        public CustomerQueries(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler<CustomerCreatedEvent>(CustomerEventHandlers.Handle);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var conn =
                new MySql.Data.MySqlClient.MySqlConnection(
                    "server=localhost;user=root;database=eventstore;port=3306;password=12345;");
            conn.Open();


            var customers = conn.Query<Customer>("select * from customers");

            conn.Close();

            return customers;
        }
    }
}
