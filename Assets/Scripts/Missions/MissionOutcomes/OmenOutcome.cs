using UnityEngine;

/// <summary>
/// Omens grant an xp buff or debuff determined by RNG
/// </summary>

[CreateAssetMenu(fileName = "OmenOutcome", menuName = "ScriptableObjects/Missions/Outcomes/OmenOutcome", order = 1)]
public class OmenOutcome : ScriptableObject
{
    public float coefficient; 
}