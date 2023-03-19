using UnityEditor.Build;
using UnityEngine;

public class MissionOutcome : ScriptableObject, IMissionOutcome
{
    [Range(0, 100)]
    public int Probability;

    [field: SerializeField]
    public bool RequiresMissionSuccess { get; set; }

    [field: SerializeField] 
    public string FlavourText { get; set; }

    public virtual void Process(ScheduledMission mission, bool isMissionModifierOutcome = false) { }
}
