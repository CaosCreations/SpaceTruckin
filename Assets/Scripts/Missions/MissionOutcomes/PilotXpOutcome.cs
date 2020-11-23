using Assets.Scripts.Singletons;
using UnityEngine;

// PilotXpOutcome can occur as a result of a mission, mission modifier, event, faction event, etc. 
[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome
{
    public int xpMin;
    public int xpMax;
	
	public override void Process(Mission mission) 
	{
		PilotsManager.Instance.pilots[mission.pilot.id].xp += Random.Range(xpMin, xpMax);
	}
}