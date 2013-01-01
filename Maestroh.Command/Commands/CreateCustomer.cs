using System;

namespace Maestroh.Command.Commands
{
    public class CreateCustomer : Command
    {
        public string CustomerName { get; set; }

        public CreateCustomer(string customerName)
        {
            CustomerName = customerName;
        }
    }
}
