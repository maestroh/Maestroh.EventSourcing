using System;
using System.Collections.Generic;
using System.Threading;

namespace Maestroh.Bus
{
    public class LocalBus
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes;

        public LocalBus()
        {
            _routes = new Dictionary<Type, List<Action<IMessage>>>();
        }


        public void RegisterHandler<TMessage>(Action<TMessage> handler) where TMessage : class, IMessage
        {
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(typeof(TMessage), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(TMessage), handlers);
            }

            handlers.Add(theMessage => handler(theMessage as TMessage));
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(typeof(TMessage), out handlers)) return;

            foreach (var handler in handlers)
            {
                var handler1 = handler;
                ThreadPool.QueueUserWorkItem(x => handler1(message));
            }
        }
    }
}
