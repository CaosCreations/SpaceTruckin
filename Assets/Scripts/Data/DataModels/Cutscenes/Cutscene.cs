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

    [field: SerializeField]
    public Cutscene CutsceneOnEnd { get; private set; }

    public override string ToString()
    {
        return $"Cutscene with name '{Name}'";
    }

    private void OnValidate()
    {
        if (CutsceneOnEnd == this)
        {
            CutsceneOnEnd = null;
            Debug.LogError($"{ToString()}'s {nameof(CutsceneOnEnd)} value is itself. This would lead to an infinite loop.");
        }
    }
}
