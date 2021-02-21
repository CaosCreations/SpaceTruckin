using UnityEngine;

[CreateAssetMenu(fileName = "MissionOutcomeContainer", menuName = "ScriptableObjects/Missions/Outcomes/MissionOutcomeContainer", order = 1)]
public class MissionOutcomeContainer : ScriptableObject
{
    public MissionOutcome[] missionOutcomes;
}
