﻿using UnityEngine;

public static class BlackjackConstants 
{ 
	// Dimensions
	public static int tableSpacing = 32;
	public static int tablePaddingLeft = 32;
	public static Vector2 cardCellSize = new Vector2(60f, 84f); 
	public static Vector2 buttonCellSize = new Vector2(128f, 32f);
	public static (Vector2, Vector2) playerCardContainerAnchors = (new Vector2(0.25f, 0.05f), new Vector2(0.75f, 0.25f));
	public static (Vector2, Vector2) npcPlayer1CardContainerAnchors = (new Vector2(0.05f, 0.25f), new Vector2(0.25f, 0.75f));
	public static (Vector2, Vector2) npcPlayer2CardContainerAnchors = (new Vector2(0.75f, 0.25f), new Vector2(0.95f, 0.75f));
	public static (Vector2, Vector2) dealerCardContainerAnchors = (new Vector2(0.25f, 0.75f), new Vector2(0.75f, 0.95f));

	// GameObject names 
	public static string headerName = "BlackjackHeader"; 
	public static string gameInfoName = "BlackjackGameInfo"; 
	public static string tableContainerName = "BlackjackTableContainer"; 
	public static string playerContainerName = "PlayerCardContainer"; 
	public static string playerTotalName = "PlayerTotalText"; 
	public static string dealerContainerName = "DealerCardContainer";
	public static string dealerTotalName = "DealerTotalText"; 
	public static string buttonContainerName = "BlackjackButtonContainer"; 
	public static string newSessionButtonName = "NewBlackjackSessionButton"; 
	public static string hitButtonName = "HitButton"; 
	public static string standButtonName = "StandButton"; 
	public static string newGameButtonName = "NewGameButton"; 
	public static string quitGameButtonName = "QuitGameButton";  
	
	// Text 
	public static string headerText = "Blackjack Table";
	public static string newSessionButtonText = "Play Blackjack"; 
	public static string hitButtonText = "Hit!";
	public static string standButtonText = "Stand";
	public static string newGameButtonText = "Play again";
	public static string quitGameButtonText = "Leave table";
	public static string dealerUnknownTotalText = "Dealer total: ?"; 
	
	// Numeric values 
	public static int lowWager = 100; 
	public static int mediumWager = 500;
	public static int highWager = 1000; 
	public static int playerStartingChips = 2500;
	public static int atBlackjackValue = 21;
	public static int bustThreshold = 22;
	public static int atDealerForcedToStandValue = 17;
	public static float winCoefficient = 1.5f;
	public static float blackjackCoefficient = 2f;

	// Sprite paths 
	public static string cardsFolderPath = "Sprites/Minigames/Blackjack/PlayingCards";
	public static string cardbackPath = "Sprites/Minigames/Blackjack/Cardbacks/cardback1";

	// Misc  
	public static string fontName = "Arial.ttf"; 
	public static Color tableColor = Color.green; 
}