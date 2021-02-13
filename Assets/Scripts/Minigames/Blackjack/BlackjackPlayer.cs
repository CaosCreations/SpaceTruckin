using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackPlayer", menuName = "ScriptableObjects/Blackjack/BlackjackPlayer", order = 1)]
public class BlackjackPlayer : ScriptableObject
{
    public string playerName;
    public CardContainer cardContainer;
    public List<Card> hand = new List<Card>();
    public int handTotal;
    public int chips; 
    public int wager;
    public bool hasWagered;
    public bool isStanding;

    public virtual bool IsStanding { get => isStanding; set => isStanding = value; }
    public bool IsBust { get => handTotal >= BlackjackConstants.bustThreshold; }
    public bool IsOut { get => IsStanding || IsBust; }
    public bool HasBlackjack { get => handTotal == BlackjackConstants.atBlackjackValue; }

    public bool HasBeatenTheDealer(int dealerHandTotal)
    {
        return !IsBust && handTotal > dealerHandTotal;
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

    public void ClearCards()
    {
        cardContainer.DestroyCards();
    }

    public void ResetHand()
    {
        ClearCards();
        handTotal = 0;
        hand = new List<Card>();
        SetTotalText();
    }

    public void SetTotalText()
        => cardContainer.TotalText = $"{playerName} total: {handTotal}";

    public void Wager(int chipsToWager)
    {
        if (chips >= chipsToWager)
        {
            wager = chipsToWager;
            chips -= chipsToWager;
        }
    }

    public virtual void Wager() { }
}
