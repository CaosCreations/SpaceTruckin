using UnityEngine;

public class BlackjackNPC : BlackjackPlayer
{
    /// <summary>
    /// Some NPC's have a riskier playstyle than others, 
    /// i.e. they are more likely to hit on higher hand totals.
    /// </summary>
    public float riskTakingProbability;
    public int lowestTotalWillStandOn;

    public bool WillTakeRisk { get => Random.Range(0f, 1f) <= riskTakingProbability; }
    public bool IsOverStandingThreshold { get => handTotal >= lowestTotalWillStandOn; }

}
