using UnityEngine;

[CreateAssetMenu(fileName = "PilotsContainer", menuName = "ScriptableObjects/Pilots/PilotContainer", order = 1)]
public class PilotsContainer : ScriptableObject, IScriptableObjectContainer<Pilot>
{
    [field: SerializeField]
    public Pilot[] Elements { get; set; } 
}
