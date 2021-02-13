using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackPlayer", menuName = "ScriptableObjects/Blackjack/BlackjackNPC", order = 1)]
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

    public override void Wager()
    {
        float wagerProbability = Random.Range(0f, 1f);
        if (chips >= BlackjackConstants.highWager && wagerProbability >= 0.66f)
        {
            wager = BlackjackConstants.highWager;
        }
        else if (chips >= BlackjackConstants.mediumWager && wagerProbability >= 0.33f)
        {
            wager = BlackjackConstants.mediumWager;
        }
        else if (chips >= BlackjackConstants.lowWager)
        {
            wager = BlackjackConstants.lowWager;
        }
        else
        {
            // Considering making NPC's able to run out of chips, 
            // Then forcing them to leave the game
            // Maybe pilots could have money themselves

            ResetChips();
        }
        chips -= wager;
    }

    private void ResetChips()
    {
        chips = BlackjackConstants.npcStartingChips;
    }
}
