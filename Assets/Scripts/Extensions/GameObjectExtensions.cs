using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameObjectExtensions 
{
    public static void AddCustomEvent(this GameObject self, EventTriggerType triggerType, Action _callback)
    {
        EventTrigger trigger = self.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = triggerType
        };
        entry.callback.RemoveAllListeners();
        entry.callback.AddListener(e => _callback());
        trigger.triggers.Add(entry);
    }
}
