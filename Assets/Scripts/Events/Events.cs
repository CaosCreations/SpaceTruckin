namespace Events
{
    public class OnSceneChangeEvent
    {

    }

    public class OnEventWithParameters : IEvent
    {
        public string Parameter { get; set; }

        public OnEventWithParameters(string parameter)
        {
            Parameter = parameter;
        }
    }

    public class OnRepairsMinigameSceneLoadedEvent : IEvent
    {
        public RepairsMinigameType RepairsMinigameType { get; private set; }

        public OnRepairsMinigameSceneLoadedEvent(RepairsMinigameType repairsMinigameType)
        {
            RepairsMinigameType = repairsMinigameType;  
        }
    }
}
