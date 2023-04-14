using System;

namespace Events
{
    [Serializable]
    public class SerializedGenericKeyValueEventArgs : UnityEngine.Object
    {
        public GenericKeyValueEventArgs EventData;
    }
}
