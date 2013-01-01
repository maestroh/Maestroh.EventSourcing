using Maestroh.Command.Commands;
using Maestroh.Command.Domain;

namespace Maestroh.Command.CommandHandlers
{
    public class CustomerCommandHandlers
    {
        private readonly IRepository<Customer> _repository;

        public CustomerCommandHandlers(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateCustomer message)
        {
            var customer = Customer.CreateNew(message.CustomerName);
            _repository.Save(customer, 0);
        }
    }
}
