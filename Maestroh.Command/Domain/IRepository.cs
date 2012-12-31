using System;

namespace Maestroh.Command.Domain
{
    public interface IRepository<out T> where T : AggregateRoot
    {
        void Save(AggregateRoot aggregate, int i);
        T GetById(Guid id);
    }
}