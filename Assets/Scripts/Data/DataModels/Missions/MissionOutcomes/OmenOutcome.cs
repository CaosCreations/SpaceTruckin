using UnityEngine;

/// <summary>
    /// Omens grant an xp buff or debuff. 
    /// The one that's granted is determined by the dice roll.
/// </summary>
[CreateAssetMenu(fileName = "OmenOutcome", menuName = "ScriptableObjects/Missions/Outcomes/OmenOutcome", order = 1)]
public class OmenOutcome : MissionOutcome
{
    public float Coefficient; 
}
