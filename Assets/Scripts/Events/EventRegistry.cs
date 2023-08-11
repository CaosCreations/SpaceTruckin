using System;
using System.Collections.Generic;

namespace Events
{
    public class EventRegistry : IEventRegistry, IEventDispatcher
    {
        private readonly Dictionary<Type, IContainer> eventLookup = new();

        public void Dispatch<T>()
        {
            try
            {
                var type = typeof(T);

                if (eventLookup.TryGetValue(type, out var eventObject))
                {
                    var container = (EventContainer)eventObject;
                    container.methods?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent
        {
            var type = typeof(IEventType);

            if (eventLookup.TryGetValue(type, out var eventObject))
            {
                var container = (EventContainer<IEventType>)eventObject;
                container.methods?.Invoke(eventClass);
            }
        }

        public void Add<T>(Action action)
        {
            var type = typeof(T);
            EventContainer container;
            if (eventLookup.TryGetValue(type, out var eventObject))
            {
                container = (EventContainer)eventObject;
            }
            else
            {
                container = new EventContainer();
                eventLookup.Add(type, container);
            }

            container.methods += action;
        }

        public void Add<IEventType>(Action<IEventType> action) where IEventType : IEvent
        {
            var type = typeof(IEventType);
            EventContainer<IEventType> container;

            if (eventLookup.TryGetValue(type, out var eventObject))
            {
                container = (EventContainer<IEventType>)eventObject;
            }
            else
            {
                container = new EventContainer<IEventType>();
                eventLookup.Add(type, container);
            }

            container.methods += action;
        }

        public void Remove<T>(Action action)
        {
            var type = typeof(T);

            if (eventLookup.TryGetValue(type, out var eventObject))
            {
                var container = (EventContainer)eventObject;
                container.methods -= action;
            }
        }

        public void Remove<IEventType>(Action<IEventType> action) where IEventType : IEvent
        {
            var type = typeof(IEventType);

            if (eventLookup.TryGetValue(type, out var eventObject))
            {
                var container = (EventContainer<IEventType>)eventObject;
                container.methods -= action;
            }
        }

        public void Clear()
        {
            foreach (var container in eventLookup.Values)
            {
                container.Clear();
            }
        }

        private class EventContainer : IContainer
        {
            public Action methods;

            public void Clear()
            {
                methods = null;
            }
        }

        private class EventContainer<IEventType> : IContainer where IEventType : IEvent
        {
            public Action<IEventType> methods;

            public void Clear()
            {
                methods = null;
            }
        }

        private interface IContainer
        {
            void Clear();
        }
    }
}
