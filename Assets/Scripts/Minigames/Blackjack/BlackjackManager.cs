using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    private GameObject dealerCardContainer; // remove?

    // Play buttons
    private GameObject hitButton;
    private GameObject standButton;
    private GameObject newGameButton;
    private GameObject quitGameButton;

    // Wager buttons
    private GameObject lowWagerButton;
    private GameObject mediumWagerButton;
    private GameObject highWagerButton;

    // Game Info
    private Text gameInfo;
    //private Text playerTotal;
    private Text dealerTotal;

    // Scriptable objects 
    public BlackjackPlayer blackjackPlayer;
    public BlackjackDealer blackjackDealer;
    public BlackjackNPC blackjackNPC1;
    public BlackjackNPC blackjackNPC2; // nullcheck so we can have fewer than 2 npcs at the table. 


    // This doesn't include the dealer
    private BlackjackPlayer[] blackjackPlayers;

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
            blackjackPlayer, /*blackjackDealer,*/ blackjackNPC1, blackjackNPC2 
        };
    }

    public void InitNewSessionButton() // open bjcanvas - but how to do when transform (not pcontainer)
    {
        GameObject newSessionButton = Instantiate(blackjackButtonPrefab);
        newSessionButton.name = BlackjackConstants.newSessionButtonName;
        //newSessionButton.transform.parent = parentContainer.transform;
        newSessionButton.transform.parent = transform;
        newSessionButton.transform.localPosition = Vector2.one / 2f;

        Button button = newSessionButton.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = BlackjackConstants.newSessionButtonText;
        button.AddOnClick(SetupTable);
    }

    // if stmt for < 2 players? 
    // or handle this at newgame time so they can get up and leave/join 
    private void SetupTable()
    {
        BlackjackUtils.InitHeader(gameObject/*blackjackTableContainer*/);
        gameInfo = BlackjackUtils.InitGameInfo(blackjackTableContainer, blackjackPlayer);

        playerCardContainer = BlackjackUtils.InitCardContainer(
            gameObject, blackjackPlayer, BlackjackConstants.playerCardContainerAnchors, isHorizontal: true);

        npc1CardContainer = BlackjackUtils.InitCardContainer(
            gameObject, blackjackNPC1, BlackjackConstants.npcPlayer1CardContainerAnchors, isHorizontal: false);

        npc2CardContainer = BlackjackUtils.InitCardContainer(
            gameObject, blackjackNPC2, BlackjackConstants.npcPlayer2CardContainerAnchors, isHorizontal: false);

        dealerCardContainer = BlackjackUtils.InitCardContainer(
            gameObject, blackjackDealer, BlackjackConstants.dealerCardContainerAnchors, isHorizontal: true);

        blackjackButtonContainer = BlackjackUtils.InitButtonContainer(gameObject);
        InitButtons();
        Deck.Init();
        Deck.Shuffle();
    }

    private void HandleWager(int wager, BlackjackPlayer _blackjackPlayer)
    {
        if (wager <= _blackjackPlayer.chips)
        {
            _blackjackPlayer.wager = wager;
            _blackjackPlayer.chips -= _blackjackPlayer.wager;

            // Game begins once player has wagered 
            NewGame();
        }
        else
        {
            gameInfo.text = $"Insufficient chips! You have {_blackjackPlayer.chips} remaining.";
        }
    }

    private void HandleNPCWagers()
    {
        
    }

    private void NewGame()
    {
        blackjackNPC1.Wager();
        blackjackNPC2.Wager();

        ResetHands();
        Deck.Shuffle();
        DealStartingHands();

        TogglePlayButtons(inGame: true);
        gameInfo.text = $"Current chips: {blackjackPlayer.chips} | Current wager: {blackjackPlayer.wager}";
        dealerTotal.text = BlackjackConstants.dealerUnknownTotalText;
    }

    private void ResetHands()
    {
        blackjackPlayer.ResetHand();
        blackjackNPC1.ResetHand();
        blackjackNPC2.ResetHand();
        blackjackDealer.ResetHand();
    }

    private void DealStartingHands()
    {
        // Deal two cards to each player 
        blackjackPlayers.Select(x => DealCard(x));
        DealCard(blackjackDealer);
        DealCard(blackjackDealer);
    }

    // faceup?
    // end of game show dealer faceup 
    private Card DealCard(BlackjackPlayer _blackjackPlayer)
    {
        Card drawnCard = Deck.GetRandomCard();
        _blackjackPlayer.AddCardToHand(drawnCard);
        BlackjackUtils.InitDrawnCardObject(_blackjackPlayer, drawnCard);
        _blackjackPlayer.cardContainer.SetTotalText();
        return drawnCard;
    }

    private void PostGame()
    {
        //dealerTotal.text = $"Dealer's total: {blackjackDealer.handTotal}";
        blackjackDealer.cardContainer.SetTotalText();
        blackjackPlayers.Select(x => SetChipsAfterGame(x));
        TogglePlayButtons(inGame: false);
    }

    private int SetChipsAfterGame(BlackjackPlayer _blackjackPlayer)
    {
        float coefficient = 1f;
        if (_blackjackPlayer.HasBlackjack)
        {
            coefficient = BlackjackConstants.blackjackCoefficient;
        }
        else if (_blackjackPlayer.HasBeatenTheDealer(blackjackDealer.handTotal))
        {
            coefficient = BlackjackConstants.winCoefficient;
        }
        _blackjackPlayer.chips = Mathf.RoundToInt(_blackjackPlayer.chips * coefficient);
        return _blackjackPlayer.chips;  
    }

    private void TogglePlayButtons(bool inGame)
    {
        hitButton.SetActive(inGame);
        standButton.SetActive(inGame);
        lowWagerButton.SetActive(!inGame);
        mediumWagerButton.SetActive(!inGame);
        highWagerButton.SetActive(!inGame);
        quitGameButton.SetActive(!inGame);
    }

    private void Update()
    {
        CheckGameStatus();
    }

    private void CheckGameStatus()
    {
        if (blackjackPlayers.Any(x => !x.IsOut))
        {
            if (blackjackDealer.IsOut)
            {
                PostGame();
            }
            else
            {
                DealCard(blackjackDealer);
            }
        }
    }

    private GameObject InitWagerButton(GameObject parentObject, GameObject buttonPrefab, int chipsToWager)
    {
        GameObject buttonObject = Instantiate(buttonPrefab, parentObject.transform);
        buttonObject.name = $"Wager{chipsToWager}Button";
        Button button = buttonObject.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = $"Wager {chipsToWager}";
        button.AddOnClick(() => blackjackPlayer.Wager(chipsToWager));
        return buttonObject;
    }

    private GameObject InitPlayButton(GameObject parentObject, GameObject blackjackButtonPrefab, PlayButtonType buttonType)
    {
        GameObject playButton = Instantiate(blackjackButtonPrefab);
        playButton.name = GetButtonName(buttonType);
        playButton.transform.parent = parentObject.transform;
        playButton.SetActive(false);

        Button button = playButton.GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = GetButtonText(buttonType);
        button.AddOnClick(() => GetPlayButtonHandler(buttonType).Invoke());
        return playButton;
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
                return () => blackjackPlayer.IsStanding = true;
            case PlayButtonType.NewGame:
                return NewGame;
            case PlayButtonType.QuitGame:
                return QuitGame;
            default:
                return NewGame;
        }
    }

    private int Hit(BlackjackPlayer _blackjackPlayer)
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
        hitButton.SetActive(areActive);
        standButton.SetActive(areActive);
        newGameButton.SetActive(areActive);
        quitGameButton.SetActive(areActive);
    }

    private void QuitGame()
    {
        Destroy(blackjackTableContainer);
        // deactivate canvas
    }

    private void InitButtons()
    {
        lowWagerButton = InitWagerButton(gameObject, blackjackButtonPrefab, BlackjackConstants.lowWager);
        mediumWagerButton = InitWagerButton(gameObject, blackjackButtonPrefab, BlackjackConstants.mediumWager);
        highWagerButton= InitWagerButton(gameObject, blackjackButtonPrefab, BlackjackConstants.highWager);

        hitButton = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.Hit);
        standButton = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.Stand);
        newGameButton = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.NewGame);
        quitGameButton = InitPlayButton(blackjackButtonContainer, blackjackButtonPrefab, PlayButtonType.QuitGame);
    }

    private string GetButtonName(PlayButtonType buttonType)
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