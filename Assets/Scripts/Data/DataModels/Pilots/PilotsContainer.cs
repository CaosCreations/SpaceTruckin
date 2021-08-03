using UnityEngine;

[CreateAssetMenu(fileName = "PilotsContainer", menuName = "ScriptableObjects/PilotContainer", order = 1)]
public class PilotsContainer : ScriptableObject, IScriptableObjectContainer<Pilot>
{
    [field: SerializeField]
    public Pilot[] Elements { get; set; } 
}
