using System;
using System.Collections.Generic;
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
        
        if (self.TryGetComponent<Image>(out var image))
        {
            image.sprite = sprite;
        }
    }

    public static Color GetImageColour(this GameObject self)
    {
        
        if (self.TryGetComponent<Image>(out var image))
            return image.color;

        return Color.white;
    }

    public static Text SetText(this GameObject self, string value)
    {
        
        if (self.TryGetComponent<Text>(out var text))
            return text.SetText(value);

        return default;
    }

    public static void ParentToPlayer(this GameObject self)
    {
        self.transform.SetParent(PlayerManager.PlayerObject.transform);
    }

    public static void SetParent(this GameObject self, GameObject parent)
    {
        self.transform.SetParent(parent.transform);
    }

    public static void Orphan(this GameObject self)
    {
        self.transform.parent = null;
    }

    public static void DestroyIfExists(this GameObject self)
    {
        if (self != null)
        {
            UnityEngine.Object.Destroy(self);
        }
    }

    public static void SetLayerRecursively(this GameObject self, int newLayer)
    {
        if (self == null)
        {
            Debug.LogError("Cannot set layer recursively as game object was null");
            return;
        }

        self.layer = newLayer;

        foreach (Transform child in self.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void CentreObject(this GameObject self)
    {
        self.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width / 2,
            Screen.height / 2,
            Camera.main.nearClipPlane));
    }

    public static List<GameObject> FindParentObjectsWithTag(this GameObject self, string tag)
    {
        List<GameObject> parentsWithTag = new List<GameObject>();

        Transform transformInTree = self.transform;

        while (transformInTree.parent != null)
        {
            if (transformInTree.CompareTag(tag))
            {
                parentsWithTag.Add(transformInTree.gameObject);
            }

            transformInTree = transformInTree.parent.transform;
        }

        return parentsWithTag;
    }

    public static bool ObjectWithTagIsParent(this GameObject self, string tag)
    {
        Transform transformInTree = self.transform;

        while (transformInTree.parent != null)
        {
            if (transformInTree.CompareTag(tag))
                return true;

            transformInTree = transformInTree.parent.transform;
        }

        return false;
    }

    public static GameObject GetChildObjectWithTag(this GameObject self, string tag)
    {
        foreach (Transform child in self.transform)
        {
            if (child.CompareTag(tag))
                return child.gameObject;
        }

        return null;
    }

    public static bool ObjectWithTagIsChild(this GameObject self, string tag)
    {
        return self.GetChildObjectWithTag(tag) != null;
    }
}
