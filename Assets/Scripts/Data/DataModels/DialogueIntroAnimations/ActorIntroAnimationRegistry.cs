using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ActorIntroAnimationRegistry",
    menuName = "ScriptableObjects/DialogueIntroAnimations/ActorIntroAnimationRegistry",
    order = 1)]
public class ActorIntroAnimationRegistry : ScriptableObject, IScriptableObjectContainer<ActorIntroAnimationData>
{
    [field: SerializeField]
    public ActorIntroAnimationData[] Elements { get; set; }

    private Dictionary<string, ActorIntroAnimationData> lookup;

    public ActorIntroAnimationData GetByKey(string key)
    {
        if (lookup == null)
        {
            InitLookup();
        }

        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        lookup.TryGetValue(key, out var data);
        return data;
    }

    private void InitLookup()
    {
        lookup = new Dictionary<string, ActorIntroAnimationData>();

        if (Elements == null)
        {
            return;
        }

        foreach (var element in Elements)
        {
            if (element != null && !string.IsNullOrEmpty(element.Key))
            {
                lookup[element.Key] = element;
            }
        }
    }
}
