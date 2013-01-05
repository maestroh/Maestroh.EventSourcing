using System.Runtime.Serialization.Formatters.Binary;
using Maestroh.Bus;
using Maestroh.Command.CommandHandlers;
using Maestroh.Command.Commands;
using Maestroh.Command.Domain;
using Maestroh.Command.EventStore;
using Maestroh.Query.Service;

namespace Maestroh.Application
{
    class Program
    {
        private static IBus _bus;
        private static CustomerQueries _customerQueries;

        static void Main(string[] args)
        {
            _bus = SetupBus();
            _customerQueries = new CustomerQueries(_bus);
            ShowMenu();
        }

        private static IBus SetupBus()
        {
            _bus = new LocalBus();

            var eventAccess = new EventAccess(new BinaryFormatter());
            var eventStore = new EventStore(eventAccess);
            var repo = new Repository<Customer>(eventStore, _bus);
            var customerCommandHandlers = new CustomerCommandHandlers(repo);
            _bus.RegisterHandler<CreateCustomer>(customerCommandHandlers.Handle);

            return _bus;
        }

        private static void ShowMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("1. View GetCustomers");
            System.Console.WriteLine("2. Add Customer");
            System.Console.Write("> ");
            var selection = System.Console.ReadLine();
            while (!(selection.Equals("1") || selection.Equals("2")))
            {
                System.Console.WriteLine("Please enter a valid numeric value. The value {0} is invalid.", selection);
                selection = System.Console.ReadLine();
            }

            switch (selection)
            {
                case "1":
                    ListCustomers();
                    break;
                case "2":
                    AddCustomer();
                    break;
            }

        }

        private static void AddCustomer()
        {
            System.Console.Clear();
            System.Console.Write("Enter Customer Name: ");
            var customerName = System.Console.ReadLine();

            var createCustomerCommand = new CreateCustomer(customerName);
            _bus.Publish(createCustomerCommand);

            ShowMenu();
        }

        private static void ListCustomers()
        {
            System.Console.Clear();
            var customers = _customerQueries.GetCustomers();

            foreach (var customer in customers)
            {
                System.Console.WriteLine(string.Format("{0}\t{1}", customer.CustomerId, customer.Name));
            }

            System.Console.Write("Press any key to continue.");
            System.Console.ReadLine();
            ShowMenu();
        }
    }
}
