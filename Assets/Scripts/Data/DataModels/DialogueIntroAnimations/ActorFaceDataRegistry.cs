using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ActorFaceDataRegistry",
    menuName = "ScriptableObjects/DialogueIntroAnimations/ActorFaceDataRegistry",
    order = 1)]
public class ActorFaceDataRegistry : ScriptableObject, IScriptableObjectContainer<ActorFaceData>
{
    [field: SerializeField]
    public ActorFaceData[] Elements { get; set; }

    private Dictionary<string, ActorFaceData> lookup;

    public ActorFaceData GetByActorName(string actorName)
    {
        if (lookup == null)
        {
            InitLookup();
        }

        if (string.IsNullOrEmpty(actorName))
        {
            return null;
        }

        lookup.TryGetValue(actorName, out var data);
        return data;
    }

    private void InitLookup()
    {
        lookup = new Dictionary<string, ActorFaceData>();

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
