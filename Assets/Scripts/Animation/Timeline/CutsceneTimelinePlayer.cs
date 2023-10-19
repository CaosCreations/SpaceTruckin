using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTimelinePlayer : MonoBehaviour
{
    [field: SerializeField]
    public PlayableDirector PlayableDirector { get; private set; }

    [field: SerializeField]
    public Cutscene Cutscene { get; private set; }

    [field: SerializeField]
    public CutsceneAnimatorSettings AnimatorSettings { get; private set; } = new();
}
