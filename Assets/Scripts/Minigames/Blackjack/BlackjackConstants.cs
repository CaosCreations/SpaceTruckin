using UnityEngine;

public static class BlackjackConstants 
{ 
	// Dimensions
	public const int TableSpacing = 32;
	public const int TablePaddingLeft = 32;
	public static readonly Vector2 CardCellSize = new Vector2(60f, 84f); 
	public static readonly Vector2 ButtonCellSize = new Vector2(128f, 32f);
	
	// GameObject names 
	public static readonly string HeaderName = "BlackjackHeader"; 
	public static readonly string GameInfoName = "BlackjackGameInfo"; 
	public static readonly string TableContainerName = "BlackjackTableContainer"; 
	public static readonly string PlayerContainerName = "PlayerCardContainer"; 
	public static readonly string PlayerTotalName = "PlayerTotalText"; 
	public static readonly string DealerContainerName = "DealerCardContainer";
	public static readonly string DealerTotalName = "DealerTotalText"; 
	public static readonly string ButtonContainerName = "BlackjackButtonContainer"; 
	public static readonly string NewSessionButtonName = "NewBlackjackSessionButton"; 
	public static readonly string HitButtonName = "HitButton"; 
	public static readonly string StandButtonName = "StandButton"; 
	public static readonly string NewGameButtonName = "NewGameButton"; 
	public static readonly string QuitGameButtonName = "QuitGameButton";  
	
	// Text 
	public static readonly string HeaderText = "Blackjack Table";
	public static readonly string NewSessionButtonText = "Play Blackjack"; 
	public static readonly string HitButtonText = "Hit!";
	public static readonly string StandButtonText = "Stand";
	public static readonly string NewGameButtonText = "Play again";
	public static readonly string QuitGameButtonText = "Leave table";
	public static readonly string DealerUnknownTotalText = "Dealer total: ?"; 
	
	// Numeric values 
	public const int LowWager = 100; 
	public const int MediumWager = 500;
	public const int HighWager = 1000; 
	public const int PlayerStartingChips = 2500;

	// Sprite paths 
	public static readonly string CardsFolderPath = "Sprites/Minigames/Blackjack/PlayingCards";
	public static readonly string CardbackPath = "Sprites/Minigames/Blackjack/Cardbacks/cardback1";

	// Misc  
	public static readonly string FontName = "Arial.ttf"; 
	public static readonly Color TableColor = Color.green; 
}