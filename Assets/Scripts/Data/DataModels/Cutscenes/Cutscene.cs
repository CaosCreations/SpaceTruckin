using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene", order = 1)]
public class Cutscene : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public string OnSceneLoadName { get; private set; }

    [field: SerializeField]
    public PlayableAsset PlayableAsset { get; private set; }

    [field: SerializeField]
    public CutsceneConversationSettings ConversationSettings { get; private set; }

    public bool Played { get; set; }

    public override string ToString()
    {
        return $"Cutscene with name '{Name}'";
    }
}
