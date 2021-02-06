using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This SO allows for multiple players if a container object is added 

[CreateAssetMenu(fileName = "BlackjackPlayer", menuName = "ScriptableObjects/BlackjackPlayer", order = 1)]
public class BlackjackPlayer : ScriptableObject
{
    public BlackjackPlayerType type;
    public Transform parentTransform; // set this in init. 
    public List<Card> hand = new List<Card>(); 
    public int handTotal;
    public int lowestTotalWillStandOn;
    public int chips; 
    public int wager;

    /// <summary>
    /// Some NPC_Players have a riskier playstyle than others, 
    /// i.e. they are more likely to hit on higher hand totals.
    /// </summary>
    public float riskTakingProbability;
	
    public bool hasWagered; 
    public bool isStanding;
    public bool isBust;

    public bool IsPlayer { get => type == BlackjackPlayerType.Player; }
    public bool IsNPC_Player { get => type == BlackjackPlayerType.NPC_Player; }
    public bool IsDealer { get => type == BlackjackPlayerType.Dealer; }
    public bool IsOverStandingThreshold { get => handTotal >= lowestTotalWillStandOn; }

    public BlackjackPlayer Init()
    {
        handTotal = 0;
        hand = new List<Card>();
        isStanding = false;
        isBust = false;
        return this;
    }

    public void AddCardToHand(Card cardToAdd)
    {
        hand.Add(cardToAdd);
        handTotal += cardToAdd.value;

        // Check if ace should be high or low, and adjust total value accordingly 
        foreach (Card card in hand)
        {
            if (card.value == 1)
            {
                if (!card.isHigh && handTotal + 9 <= 21)
                {
                    card.isHigh = true;
                    handTotal += 9;
                }
                else if (card.isHigh && handTotal > 21)
                {
                    card.isHigh = false;
                    handTotal -= 9;
                }
            }
        }
    }

    public void Stand() => isStanding = true;
    public void GoBust() => isBust = true;
}
