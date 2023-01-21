namespace Events
{
    public class ScopedEventRegistry : IEventDispatcher
    {
        private readonly EventRegistry permanent = new EventRegistry();
        private readonly EventRegistry transient = new EventRegistry();

        public IEventRegistry Permanent => permanent;
        public IEventRegistry Transient => transient;

        public void Dispatch<T>()
        {
            permanent.Dispatch<T>();
            transient.Dispatch<T>();
        }

        public void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent
        {
            permanent.Dispatch(eventClass);
            transient.Dispatch(eventClass);
        }

        public void ClearTransient()
        {
            transient.Clear();
        }
    }
}