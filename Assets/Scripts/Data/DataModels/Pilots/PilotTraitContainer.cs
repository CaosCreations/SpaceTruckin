using UnityEngine;

[CreateAssetMenu(fileName = "PilotTraitContainer", menuName = "ScriptableObjects/Pilots/PilotTraitContainer", order = 1)]
public class PilotTraitContainer : ScriptableObject, IScriptableObjectContainer<PilotTrait>
{
    [field: SerializeField]
    public PilotTrait[] Elements { get; set; }
}
