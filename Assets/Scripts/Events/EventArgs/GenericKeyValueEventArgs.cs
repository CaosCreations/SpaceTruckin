using System;

namespace Events
{
    [Serializable]
    public class GenericKeyValueEventArgs
    {
        public object Key;
        public object Value;
    }
}
