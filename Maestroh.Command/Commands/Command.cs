using System;
using Maestroh.Bus;

namespace Maestroh.Command.Commands
{
    public class Command : IMessage
    {
        public Guid Id { get; set; }

        public Command(Guid id)
        {
            Id = id;
        }
    }
}
