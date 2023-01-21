using UnityEngine;

[CreateAssetMenu(fileName = "PilotSpeciesTraitContainer", menuName = "ScriptableObjects/Pilots/PilotSpeciesTraitContainer", order = 1)]
public class PilotSpeciesTraitContainer : ScriptableObject, IScriptableObjectContainer<PilotSpeciesTrait>
{
    [field: SerializeField]
    public PilotSpeciesTrait[] Elements { get; set; }
}
