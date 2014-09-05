using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace PIC
{
    public interface IServiceBus : IBus
    {
    }

    public class ServiceBus : IServiceBus
    {
        private IBus bus;

        public ServiceBus(IBus bus)
        {
            this.bus = bus;
        }

        #region IBus Method

        public IMessageContext CurrentMessageContext
        {
            get { return bus.CurrentMessageContext; }
        }

        public void DoNotContinueDispatchingCurrentMessageToHandlers()
        {
            bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }

        public void ForwardCurrentMessageTo(string destination)
        {
            bus.ForwardCurrentMessageTo(destination);
        }

        public void HandleCurrentMessageLater()
        {
            bus.HandleCurrentMessageLater();
        }

        public IDictionary<string, string> OutgoingHeaders
        {
            get { return bus.OutgoingHeaders; }
        }

        public void Publish<T>(Action<T> messageConstructor) where T : IMessage
        {
            bus.Publish<T>(messageConstructor);
        }

        public void Publish<T>(params T[] messages) where T : IMessage
        {
            bus.Publish<T>(messages);
        }

        public void Reply<T>(Action<T> messageConstructor) where T : IMessage
        {
            bus.Reply<T>(messageConstructor);
        }

        public void Reply(params IMessage[] messages)
        {
            bus.Reply(messages);
        }

        public void Return(int errorCode)
        {
            bus.Return(errorCode);
        }

        public void Send<T>(string destination, string correlationId, Action<T> messageConstructor) where T : IMessage
        {
            bus.Send<T>(destination, correlationId, messageConstructor);
        }

        public void Send(string destination, string correlationId, params IMessage[] messages)
        {
            bus.Send(destination, correlationId, messages);
        }

        public ICallback Send<T>(string destination, Action<T> messageConstructor) where T : IMessage
        {
            return bus.Send<T>(destination, messageConstructor);
        }

        public ICallback Send(string destination, params IMessage[] messages)
        {
            return bus.Send(destination, messages);
        }

        public ICallback Send<T>(Action<T> messageConstructor) where T : IMessage
        {
            return bus.Send<T>(messageConstructor);
        }

        public ICallback Send(params IMessage[] messages)
        {
            return bus.Send(messages);
        }

        public void SendLocal<T>(Action<T> messageConstructor) where T : IMessage
        {
            bus.SendLocal<T>(messageConstructor);
        }

        public void SendLocal(params IMessage[] messages)
        {
            bus.SendLocal(messages);
        }

        public void Subscribe<T>(Predicate<T> condition) where T : IMessage
        {
            bus.Subscribe(condition);
        }

        public void Subscribe(Type messageType, Predicate<IMessage> condition)
        {
            bus.Subscribe(messageType, condition);
        }

        public void Subscribe<T>() where T : IMessage
        {
            bus.Subscribe<T>();
        }

        public void Subscribe(Type messageType)
        {
            bus.Subscribe(messageType);
        }

        public void Unsubscribe<T>() where T : IMessage
        {
            bus.Unsubscribe<T>();
        }

        public void Unsubscribe(Type messageType)
        {
            bus.Unsubscribe(messageType);
        }

        public object CreateInstance(Type messageType)
        {
            return bus.CreateInstance(messageType);
        }

        public T CreateInstance<T>(Action<T> action) where T : IMessage
        {
            return bus.CreateInstance<T>(action);
        }

        public T CreateInstance<T>() where T : IMessage
        {
            return bus.CreateInstance<T>();
        }

        #endregion
    }
}
