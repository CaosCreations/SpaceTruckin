using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneContainer", menuName = "ScriptableObjects/Cutscenes/CutsceneContainer", order = 1)]
public class CutsceneContainer : ScriptableObject, IScriptableObjectContainer<Cutscene>
{
    [field: SerializeField]
    public Cutscene[] Elements { get; set; }
}
