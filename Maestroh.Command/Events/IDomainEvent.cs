using System;
using Maestroh.Bus;

namespace Maestroh.Command.Events
{
    public interface IDomainEvent : IMessage
    {
        Guid AggregateId { get; set; }
        int Version { get; set; }
    }
}
