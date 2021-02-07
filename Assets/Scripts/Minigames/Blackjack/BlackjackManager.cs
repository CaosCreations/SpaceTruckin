﻿using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum BlackjackPlayerType
{
    Player, NPC_Player, Dealer
}

public class BlackjackManager : MonoBehaviour
{
    // Prefabs 
    public GameObject blackjackButtonPrefab;

    // Container objects 
    [SerializeField]
    private GameObject parentContainer;
    private GameObject blackjackButtonContainer;
    private GameObject blackjackTableContainer;
    private GameObject playerCardContainer;
    private GameObject npc1CardContainer;
    private GameObject npc2CardContainer;
    private GameObject dealerCardContainer;

    // Play buttons
    private GameObject hitButtonObject;
    private GameObject standButtonObject;
    private GameObject newGameButtonObject;
    private GameObject quitGameButtonObject;

    // Wager buttons
    private GameObject lowWagerObject;
    private GameObject mediumWagerObject;
    private GameObject highWagerObject;

    // Game Info
    private Text gameInfo;
    private Text playerTotal;
    private Text dealerTotal;

    // Scriptable objects 
    public BlackjackPlayer blackjackPlayer;
    public BlackjackDealer blackjackDealer;
    public BlackjackNPC blackjackNPC1;
    public BlackjackNPC blackjackNPC2; // nullcheck so we can have fewer than 2 npcs at the table. 

    private BlackjackPlayer[] blackjackPlayers;

    //public BlackjackPlayer[] blackjackPlayers; // does this include dealer?
    //private BlackjackPlayer Dealer { get => blackjackPlayers.FirstOrDefault(x => x.IsDealer); }

    //private readonly System.Random RNG = new System.Random();

    // Multiple by these numbers to calculate winnings 
    // Win gives 1.5 times your wager back
    // Blackjack doubles your money 
    private float winCoefficient = 1.5f;
    private float blackjackCoefficient = 2f;

    public enum PlayButtonType
    {
        Hit, Stand, NewGame, QuitGame
    }

    private void Start()
    {
        InitNewSessionButton();
        blackjackPlayer.chips = BlackjackConstants.playerStartingChips;
        blackjackPlayers = new BlackjackPlayer[] 
        { 
            blackjackPlayer, blackjackDealer, blackjackNPC1, blackjackNPC2 
        };
    }

    public void InitNewSessionButton()
    {
        GameObject newSessionButtonObject = Instantiate(blackjackButtonPrefab);
        newSessionButtonObject.name = BlackjackConstants.newSessionButtonName;
        newSessionButtonObject.transform.parent = parentContainer.transform;
        newSessionButtonObject.transform.localPosition = Vector2.one / 2f;

        Button newSessionButton = newSessionButtonObject.GetComponent<Button>();
        newSessionButton.GetComponentInChildren<Text>().text = BlackjackConstants.newSessionButtonText;
        newSessionButton.AddOnClick(SetupTable);
    }

    // if stmt for < 2 players? 
    // or handle this at newgame time so they can get up and leave/join 
    private void SetupTable()
    {
        blackjackTableContainer = BlackjackUtils.InitTableContainer(parentContainer);

        BlackjackUtils.InitHeader(blackjackTableContainer);
        gameInfo = BlackjackUtils.InitGameInfo(blackjackTableContainer, blackjackPlayer);

        // encap. this 
        playerCardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, blackjackPlayer);
        playerTotal = BlackjackUtils.InitTotalText(blackjackPlayer);
        dealerCardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, blackjackDealer);
        dealerTotal = BlackjackUtils.InitTotalText(blackjackDealer);
        npc1CardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, blackjackNPC1);
        playerTotal = BlackjackUtils.InitTotalText(blackjackNPC1);
        npc2CardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, blackjackNPC2);
        dealerTotal = BlackjackUtils.InitTotalText(blackjackNPC2);

        blackjackButtonContainer = BlackjackUtils.InitButtonContainer(blackjackTableContainer);

        lowWagerObject = InitWagerButtonObject(BlackjackConstants.lowWager);
        mediumWagerObject = InitWagerButtonObject(BlackjackConstants.mediumWager);
        highWagerObject = InitWagerButtonObject(BlackjackConstants.highWager);

        hitButtonObject = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.Hit);
        standButtonObject = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.Stand);
        newGameButtonObject = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.NewGame);
        quitGameButtonObject = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.QuitGame);

        blackjackDealer.type = BlackjackPlayerType.Dealer;
        Deck.Init();
        Deck.Shuffle();
    }

    private GameObject InitWagerButtonObject(int wager)
    {
        GameObject buttonObject = Instantiate(blackjackButtonPrefab, blackjackButtonContainer.transform);
        buttonObject.name = $"Wager{wager}Button";
        Button button = buttonObject.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = $"Wager {wager}";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { HandleWager(wager); });
        return buttonObject;
    }

    private void HandleWager(int wager)
    {
        if (wager <= blackjackPlayer.chips)
        {
            blackjackPlayer.wager = wager;
            blackjackPlayer.chips -= blackjackPlayer.wager;

            // Game begins once player has wagered 
            NewGame();
        }
        else
        {
            gameInfo.text = $"Insufficient chips! You have {blackjackPlayer.chips} remaining.";
        }
    }

    private void NewGame()
    {
        ClearCards();
        Deck.Shuffle();


        // newgame vs. resetgame (dont call init again)

        TogglePlayButtons(inGame: true);
        DealStartingHands();

        gameInfo.GetComponent<Text>().text = $"Current chips: {blackjackPlayer.chips} | Current wager: {blackjackPlayer.wager}";
        dealerTotal.text = BlackjackConstants.dealerUnknownTotalText;
    }

    private void ClearCards()
    {
        blackjackPlayer.ClearCards();
        blackjackDealer.ClearCards();
        blackjackNPC1.ClearCards();
        blackjackNPC2.ClearCards();
    }

    private void DealStartingHands()
    {
        // Deal two cards to each player 
        DealCard(blackjackPlayer);
        DealCard(blackjackPlayer);
        DealCard(blackjackNPC1);
        DealCard(blackjackNPC1);
        DealCard(blackjackNPC2);
        DealCard(blackjackNPC2);
        DealCard(blackjackDealer);
        DealCard(blackjackDealer);

        //blackjackPlayers.Select(x => { DealCard(x); DealCard(x); });
    }

    private Card DealCard(BlackjackPlayer _blackjackPlayer)
    {
        Card drawnCard = Deck.GetRandomCard();
        _blackjackPlayer.AddCardToHand(drawnCard);
        BlackjackUtils.InitDrawnCardObject(_blackjackPlayer, drawnCard);

        // Don't show the dealer's total  
        // use interface here. 
        if (!_blackjackPlayer.IsDealer)
        {
            _blackjackPlayer.cardContainer.SetTotalText(_blackjackPlayer.handTotal);
        }
        return drawnCard;
    }

    //private bool PlayerWillTakeRisk(BlackjackPlayer _blackjackPlayer)
    //{
    //    return RNG.NextDouble() <= _blackjackPlayer.riskTakingProbability;
    //}

    private bool AllPlayersAreOut() // redundant?
    {
        //return blackjackPlayers.Where(x => !x.IsDealer).Any(x => x.isBust || x.isStanding);
        return blackjackPlayers.Any(x => !(x.IsBust || x.isStanding));
    }

    private bool DealerIsOut()
    {
        // could prop encap.
        return !(blackjackDealer.isStanding || blackjackDealer.isBust);
    }

    private void PostGame()
    {
        dealerTotal.text = $"Dealer's total: {blackjackDealer.handTotal}"; // change to cardcontainer

        // Reset hand totals and cards to starting values
        //blackjackPlayer.ResetHand();
        //blackjackNPC1.ResetHand();
        //blackjackNPC2.ResetHand();
        //blackjackDealer.ResetHand();

        //if ()


        if (!blackjackPlayer.isBust)
        {
            if (blackjackDealer.isBust)
            {
                playerWins = true;
            }
            else
            {
                if (blackjackPlayer.handTotal > blackjackDealer.handTotal)
                {
                    playerWins = true;
                }
            }
        }

        TogglePlayButtons(inGame: false);

        if (playerWins)
        {
            blackjackPlayer.chips += 2 * blackjackPlayer.wager;

            // Since the player has to wager after each round now, 
            // we need to display their available chips 
            gameInfo.text = $"Player wins {blackjackPlayer.wager} chips! Current stack: {blackjackPlayer.chips}";
        }
        else
        {
            gameInfo.text = $"Player loses {blackjackPlayer.wager} chips! Current stack: {blackjackPlayer.chips}";
        }

        // new logic: 
        foreach (BlackjackPlayer blackjackPlayer in blackjackPlayers.Where(x => !x.IsDealer))
        {
            if (PlayerHasWon(blackjackPlayer))
            {
                blackjackPlayer.chips += 2 * blackjackPlayer.wager; // encap. and make coefficient dynamic.

                // update game info text (for each player or just player?)

            }
        }

    }

    private bool PlayerHasWon(BlackjackPlayer blackjackPlayer)
    {
        // wrong - BJD could be bust as well as player (after - time makes sense?)
        return blackjackDealer.IsBust
            || !blackjackDealer.IsBust && blackjackPlayer.handTotal > blackjackDealer.handTotal;
    }

    private void TogglePlayButtons(bool inGame)
    {
        hitButtonObject.SetActive(inGame);
        standButtonObject.SetActive(inGame);
        lowWagerObject.SetActive(!inGame);
        mediumWagerObject.SetActive(!inGame);
        highWagerObject.SetActive(!inGame);
        quitGameButtonObject.SetActive(!inGame);
    }

    private void Update()
    {
        // Check if it's the dealer's turn to draw cards 
        if (AllPlayersAreOut() && !DealerIsOut())
        {
            DealCard(blackjackDealer);
            // wait delay WFS
        }

        CheckGameStatus();
    }

    private void DealToDealer()
    {
        while (!DealerIsOut())
        {
            DealCard(blackjackDealer);
        }
    }

    private void CheckGameStatus()
    {
        // set button inter too?
        // determine other stuff taht can be abastracted

        if (AllPlayersAreOut() && DealerIsOut())
        {
            PostGame(); // rename
        }

    }

    public GameObject InitPlayButton(GameObject parentObject, GameObject blackjackButtonPrefab, PlayButtonType buttonType)
    {
        GameObject gameObject = Instantiate(blackjackButtonPrefab);
        gameObject.name = GetButtonName(buttonType);
        gameObject.transform.parent = parentObject.transform;
        gameObject.SetActive(false);

        Button button = gameObject.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = GetButtonText(buttonType);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate
        {
            GetPlayButtonHandler(buttonType).Invoke();
        });

        return gameObject;
    }

    public UnityAction GetPlayButtonHandler(PlayButtonType buttonType)
    {
        switch (buttonType)
        {
            case PlayButtonType.Hit:
                return () =>
                {
                    Hit(blackjackPlayer);
                    DealToNPCs();
                };
            case PlayButtonType.Stand:
                //return blackjackPlayer.Stand;
                return () => blackjackPlayer.isStanding = true;
            case PlayButtonType.NewGame:
                return NewGame;
            case PlayButtonType.QuitGame:
                return QuitGame;
            default:
                return NewGame;
        }
    }

    private void Hit(BlackjackPlayer _blackjackPlayer)
    {
        DealCard(_blackjackPlayer);
        return _blackjackPlayer.handTotal;
    }

    private void DealToNPCs()
    {
        SetButtonInteractability(false);
        if (!blackjackNPC1.IsOut)
        {
            Hit(blackjackNPC1); // make void 
        }
        if (!blackjackNPC2.IsOut)
        {
            Hit(blackjackNPC2);
        }
        SetButtonInteractability(true);
    }

    private void SetButtonInteractability(bool areActive)
    {
        hitButtonObject.SetActive(areActive);
        standButtonObject.SetActive(areActive);
        newGameButtonObject.SetActive(areActive);
        quitGameButtonObject.SetActive(areActive);
    }

    private void QuitGame()
    {
        Destroy(blackjackTableContainer);
    }

    public string GetButtonName(PlayButtonType buttonType)
    {
        switch (buttonType)
        {
            case PlayButtonType.Hit:
                return BlackjackConstants.hitButtonName;
            case PlayButtonType.Stand:
                return BlackjackConstants.standButtonName;
            case PlayButtonType.NewGame:
                return BlackjackConstants.newGameButtonName;
            case PlayButtonType.QuitGame:
                return BlackjackConstants.quitGameButtonName;
            default:
                Debug.Log("Button type does not exist.");
                return "";
        }
    }

    public string GetButtonText(PlayButtonType buttonType)
    {
        switch (buttonType)
        {
            case PlayButtonType.Hit:
                return BlackjackConstants.hitButtonText;
            case PlayButtonType.Stand:
                return BlackjackConstants.standButtonText;
            case PlayButtonType.NewGame:
                return BlackjackConstants.newGameButtonText;
            case PlayButtonType.QuitGame:
                return BlackjackConstants.quitGameButtonText;
            default:
                Debug.Log("Button type does not exist.");
                return "";
        }
    }
}