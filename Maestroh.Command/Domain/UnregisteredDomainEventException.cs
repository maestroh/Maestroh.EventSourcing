using System;

namespace Maestroh.Command.Domain
{
    class UnregisteredDomainEventException : Exception
    {
        public UnregisteredDomainEventException(string message) : base(message)
        {
        }
    }
}
