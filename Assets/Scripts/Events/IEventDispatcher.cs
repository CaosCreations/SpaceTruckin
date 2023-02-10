namespace Events
{
    public interface IEventDispatcher
    {
        void Dispatch<T>();
        void Dispatch<IEventType>(IEventType eventClass) where IEventType : IEvent;
    }
}