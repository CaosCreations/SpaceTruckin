using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome
{
    [SerializeField] private int xpMin;
    [SerializeField] private int xpMax;
    public int XpMin { get => xpMin; set => xpMin = value; }
    public int XpMax { get => xpMax; set => xpMax = value; }

    public override void Process(Mission mission) 
	{
        // Store the pilot's level before the xp is awarded.
        // Then we can check if they levelled up as a result of the mission.
        mission.MissionToArchive.PilotLevelAtTimeOfMission = mission.Pilot.Level;

        double xpGained = Random.Range(xpMin, xpMax) * ApplyOmens(mission);
        mission.MissionToArchive.TotalPilotXpGained += PilotsManager.AwardXp(mission.Pilot, xpGained);
	}

    /// <summary>
    /// Calculate the xp multiplier based on probability. 
    /// It can be below 1, resulting in an xp debuff. 
    /// </summary>
    /// <param name="mission"></param>
    /// <returns>A number by which the xp gained will be multiplied.</returns>
    private float ApplyOmens(Mission mission)
    {
        float coefficient = 1f;

        for (int i = 0; i < mission.Outcomes.Length; i++)
        {
            if (mission.Outcomes[i] is OmenOutcome)
            {
                OmenOutcome omen = mission.Outcomes[i] as OmenOutcome;
                coefficient += probability >= Random.Range(0f, 1f) ? omen.coefficient : omen.coefficient * -1;
            }
        }
        return coefficient;
    }
}