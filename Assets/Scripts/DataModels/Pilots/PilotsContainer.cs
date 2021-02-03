using UnityEngine;

[CreateAssetMenu(fileName = "PilotsContainer", menuName = "ScriptableObjects/PilotContainer", order = 1)]
public class PilotsContainer : ScriptableObject
{
    public Pilot[] pilots; 
}
