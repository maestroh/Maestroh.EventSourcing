using System;

namespace Maestroh.Command.Commands
{
    public class CreateCustomer : Command
    {
        public string CustomerName { get; set; }

        public CreateCustomer(Guid id, string customerName)
            : base(id)
        {
            CustomerName = customerName;
        }
    }
}
