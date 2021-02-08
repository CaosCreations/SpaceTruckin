using System.Collections.Generic;
using UnityEngine;

public interface IBlackjackPlayer
{
    // dealstartinghands solution
    // checkhandtotal 

    void CheckHandTotal(); // rename (it determines outcome)
}

[CreateAssetMenu(fileName = "BlackjackPlayer", menuName = "ScriptableObjects/BlackjackPlayer", order = 1)]
public class BlackjackPlayer : ScriptableObject, IBlackjackPlayer
{
    public string playerName;
    public CardContainer cardContainer;
    public List<Card> hand = new List<Card>();
    public int handTotal;
    public int chips; 
    public int wager;
    public bool hasWagered;
    public bool isStanding;
    public bool isBust;

    // replaces checkhand duplication
    public bool IsBust { get => handTotal >= BlackjackConstants.bustThreshold; }
    public bool IsStanding { get => isStanding; set => isStanding = value; }
    public bool IsOut { get => !(isStanding || isBust); }

    // ? 
    public bool HasBlackjack { get => handTotal == BlackjackConstants.atBlackjackValue; }
    
    // needs to be a method (dont have access to dealer)
    //public bool HasBeatenTheDealer { get => !HasBlackjack && handTotal > dealer }

    public bool HasBeatenTheDealer(int dealerHandTotal)
    {
        return !IsBust && handTotal > dealerHandTotal;
        // draws?
        // support draw with generic player vs. dealer method*
    }

    public BlackjackPlayer Init(CardContainer cardContainer)
    {
        //handTotal = 0;
        //hand = new List<Card>();
        //isStanding = false;
        //isBust = false;
        this.cardContainer = cardContainer;
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

    public void ClearCards()
    {
        cardContainer.DestroyCards();
    }

    // rename - determinestatus? 
    public virtual void CheckHandTotal() // return bool for better sense in manager
    {
        //if (handTotal >= BlackjackConstants.bustThreshold)
        //{
        //    isBust = true;
        //    return;
        //}

        if (handTotal == BlackjackConstants.atBlackjackValue)
        {
            isStanding = true;
        }
    }

    public void ResetHand()
    {
        ClearCards();
        handTotal = 0;
        hand = new List<Card>();
        cardContainer.SetTotalText();
    }

    public void Wager(int chipsToWager)
    {
        if (chips >= chipsToWager)
        {
            wager = chipsToWager;
            chips -= chipsToWager;
        }
    }
}
