using UnityEngine;

// Cannot think of a better name for this right now

/// <summary>
///     Holds the <see cref="PilotTrait"/>'s that affect this <see cref="Mission"/>,
///     and whether they affect it positively or negatively.
/// </summary>
[CreateAssetMenu(fileName = "MissionPilotTraitEffects",
    menuName = "ScriptableObjects/Missions/MissionPilotTraitEffects", order = 1)]
public class MissionPilotTraitEffects : ScriptableObject
{
    [SerializeField] private MissionPilotTraitEffect[] missionPilotTraitEffects;
    public MissionPilotTraitEffect[] PilotTraitEffects => missionPilotTraitEffects;
}
