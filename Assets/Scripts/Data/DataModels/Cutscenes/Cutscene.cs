using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene", order = 1)]
public class Cutscene : ScriptableObject
{
    public string Name;
    public string OnSceneLoadName;
    public PlayableAsset PlayableAsset;
    public bool IsDialogueCutscene;
}
