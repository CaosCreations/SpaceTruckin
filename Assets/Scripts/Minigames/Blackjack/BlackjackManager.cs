using System.Linq;
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
    public BlackjackPlayer blackjackDealer;

	public BlackjackPlayer[] blackjackPlayers; // does this include dealer?

	private readonly System.Random RNG = new System.Random();

	public enum PlayButtonType
	{
		Hit, Stand, NewGame, QuitGame
	}

	private void Start()
    {
        InitNewSessionButton(); 
		blackjackPlayer.chips = BlackjackConstants.playerStartingChips;
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
	private void SetupTable()
	{
		blackjackTableContainer = BlackjackUtils.InitTableContainer(parentContainer);

        BlackjackUtils.InitHeader(blackjackTableContainer);
		gameInfo = BlackjackUtils.InitGameInfo(blackjackTableContainer, blackjackPlayer);

		playerCardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, isDealer: false);
		playerTotal = BlackjackUtils.InitTotalText(playerCardContainer, isDealer: false);
		dealerCardContainer = BlackjackUtils.InitCardContainer(blackjackTableContainer, isDealer: true);
		dealerTotal = BlackjackUtils.InitTotalText(dealerCardContainer, isDealer: true);

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
		Deck.Shuffle();

		DestroyChildCardObjects(playerCardContainer.transform);
        DestroyChildCardObjects(dealerCardContainer.transform);

        blackjackPlayer.Init();
		blackjackDealer.Init();

		TogglePlayButtons(inGame: true);

		// Deal starting hands 
		DealCard(blackjackPlayer, faceUp: true);
		DealCard(blackjackDealer, faceUp: false);
		DealCard(blackjackPlayer, faceUp: true);
		DealCard(blackjackDealer, faceUp: true);

        gameInfo.GetComponent<Text>().text = $"Current chips: {blackjackPlayer.chips} | Current wager: {blackjackPlayer.wager}";
        dealerTotal.text = BlackjackConstants.dealerUnknownTotalText;
    }

	// give generic name? 
	private void DealCard(BlackjackPlayer _blackjackPlayer, bool faceUp)
    {
		Card drawnCard = Deck.GetRandomCard();
		_blackjackPlayer.AddCardToHand(drawnCard);

		// util for creating cards.
        GameObject drawnCardObject = new GameObject("CardObject");
        drawnCardObject.transform.parent = _blackjackPlayer.IsDealer ? dealerCardContainer.transform : playerCardContainer.transform;
        drawnCardObject.AddComponent<Image>().sprite = faceUp ? drawnCard.sprite : Deck.cardbackSprite;

		// Don't show the dealer's total  
		if (!_blackjackPlayer.IsDealer) 
		{
            playerTotal.text = $"Your total: {blackjackPlayer.handTotal}";
        }

		// Check if bust 
		if (_blackjackPlayer.handTotal > 21)
		{
			_blackjackPlayer.GoBust();
		}

		// Automatically stand the player if they get blackjack 
		else if (_blackjackPlayer.handTotal == 21)
        {
			// _ or p
			_blackjackPlayer.Stand(); // rename... or get based on BJPType
        }

		// Automatically stand the player if they are not willing to take the risk
		else if (_blackjackPlayer.IsNPC_Player 
			&& _blackjackPlayer.IsOverStandingThreshold && !PlayerWillTakeRisk(_blackjackPlayer))
        {
			_blackjackPlayer.Stand();
        }

		// Dealer must stand if his total is 17 or over
		else if (_blackjackPlayer.IsDealer && _blackjackPlayer.handTotal >= 17)
        {
			_blackjackPlayer.Stand();
        }

		// End the game when all players are bust or standing
		if (AllPlayersAreOut() && DealerIsOut())
        {
			PostGame();
        }
    }

	private bool PlayerWillTakeRisk(BlackjackPlayer _blackjackPlayer)
	{
		return RNG.NextDouble() <= _blackjackPlayer.riskTakingProbability;
	}

	private bool AllPlayersAreOut()
    {
		return blackjackPlayers.Any(x => x.isBust || x.isStanding);
    }

	private bool DealerIsOut()
    {
		return blackjackDealer.isBust || blackjackDealer.isStanding;

	}

	private void PostGame()
	{
        dealerTotal.text = $"Dealer's total: {blackjackDealer.handTotal}";
        bool playerWins  = false;

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
		if (AllPlayersAreOut())
		{
			DealCard(blackjackDealer, faceUp: true);

			//// put this elsewhere? (dealcard)
			//if (blackjackDealer.handTotal >= 17 && blackjackDealer.handTotal <= 21)
			//{
			//	blackjackDealer.Stand();
			//}
		}
	}

	public void DestroyChildCardObjects(Transform _transform) 
	{ 
		foreach (Transform child in _transform) 
		{
			if (child.gameObject.GetComponent<Image>()) 
			{
				Destroy(child.gameObject); 
			}
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
		button.onClick.AddListener(delegate {
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
					TakeNPC_PlayerTurns();
				};
			case PlayButtonType.Stand:
				return blackjackPlayer.Stand;
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
		DealCard(_blackjackPlayer, faceUp: _blackjackPlayer.type != BlackjackPlayerType.Dealer);
		return _blackjackPlayer.handTotal;
    }

	private void TakeNPC_PlayerTurns() // rename
    {
		SetButtonInteractability(false);

		// Hit all players of NPC_Player type 
		blackjackPlayers.Where(x => x.type == BlackjackPlayerType.NPC_Player)
			.Select(x => Hit(x));

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