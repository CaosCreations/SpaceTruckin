using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class GameObjectExtensions 
{
    public static void AddCustomEvent(this GameObject self, EventTriggerType triggerType, Action callback)
    {
        EventTrigger trigger = self.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = triggerType
        };
        entry.callback.RemoveAllListeners();
        entry.callback.AddListener(e => callback());
        trigger.triggers.Add(entry);
    }

    public static GameObject ScaffoldUI(this GameObject self, string name, GameObject parent, (Vector2, Vector2) anchors)
    {
        self.name = name;
        self.transform.parent = parent.transform;
        RectTransform rectTransform = self.AddComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.SetAnchors(anchors);
        return self; 
    }

    public static void SetSprite(this GameObject self, Sprite sprite)
    {
        Image image = self.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite; 
        }
    }
}
