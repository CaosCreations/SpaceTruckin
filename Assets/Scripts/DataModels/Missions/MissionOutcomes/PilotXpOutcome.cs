using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome
{
    public int xpMin;
    public int xpMax;
	
	public override void Process(Mission mission) 
	{
		PilotsManager.Instance.AwardXp(mission.Ship, Mathf.FloorToInt(Random.Range(xpMin, xpMax) * ApplyOmens(mission)));
	}

    // A mission can have multiple omens attached to it
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