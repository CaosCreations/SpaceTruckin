using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene", order = 1)]
public class Cutscene : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public PlayableAsset PlayableAsset { get; private set; }

    [field: SerializeField]
    public CutsceneConversationSettings ConversationSettings { get; private set; } = new();

    [field: SerializeField]
    public Date Date { get; private set; }

    [field: SerializeField]
    public UICanvasType CanvasTypeOnEnd { get; private set; } = UICanvasType.None;

    [field: SerializeField]
    public UICanvasType CanvasTutorialTypeOnEnd { get; private set; } = UICanvasType.None;

    public override string ToString()
    {
        return $"Cutscene with name '{Name}'";
    }
}
