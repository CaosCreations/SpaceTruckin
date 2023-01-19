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
}
