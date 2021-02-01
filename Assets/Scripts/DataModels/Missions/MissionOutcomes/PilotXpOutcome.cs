using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome
{
    public int xpMin;
    public int xpMax;
	
	public override void Process(Mission mission) 
	{
        double xpGained = Mathf.FloorToInt(Random.Range(xpMin, xpMax) * ApplyOmens(mission));
        mission.MissionToArchive.TotalPilotXpGained += mission.Pilot.GainXp(xpGained);

        // Todo: report whether pilot levelled up yesterday.
        // Store flag on archived model.

        // Compare current mission (lookup first) with archived mission field
            // Might need to store the pilot's current level on the archived mission too

        // So when to call LevelUp?
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