using System;

namespace Events
{
    public interface IEventRegistry
    {
        void Add<T>(Action action);
        void Add<IEventType>(Action<IEventType> action) where IEventType : IEvent;

        void Remove<T>(Action action);
        void Remove<IEventType>(Action<IEventType> action) where IEventType : IEvent;
    }
}