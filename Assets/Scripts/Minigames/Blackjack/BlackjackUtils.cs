using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlackjackUtils : MonoBehaviour
{
    public static void SetupListeners(Button button, UnityAction handler)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(handler);
    }

    //public static GameObject InitTable/*Container*/(GameObject parentObject, Dictionary<BlackjackPlayer, GameObject>)
    //{
    //    playerContainer = InitCardContainer(parentObject,)

    //    return blackjackTableContainer;
    //}

    public static void InitHeader(GameObject parentObject)
    {
        GameObject headerTextObject = new GameObject(BlackjackConstants.headerName);
        headerTextObject.transform.parent = parentObject.transform;
        Text headerText = headerTextObject.ScaffoldTextComponent();
        headerText.text = BlackjackConstants.headerText;
    }

    public static Text InitGameInfo(GameObject parentObject, BlackjackPlayer blackjackPlayer)
    {
        GameObject gameInfoObject = new GameObject(BlackjackConstants.gameInfoName);
        gameInfoObject.transform.parent = parentObject.transform;
        Text gameInfoText = gameInfoObject.ScaffoldTextComponent();
        gameInfoText.text = $"You have {blackjackPlayer.chips} chips. Place a bet to begin the game!";
        return gameInfoText;
    }

    public static GameObject InitCardContainer(GameObject parentObject, BlackjackPlayer blackjackPlayer, (Vector2, Vector2) anchors, bool isHorizontal)
    {
        string cardContainerName = GetObjectName(blackjackPlayer, "CardContainer");
        GameObject cardContainerObject = new GameObject().ScaffoldUI(cardContainerName, parentObject, anchors);

        HorizontalOrVerticalLayoutGroup layoutGroup;
        if (isHorizontal)
        {
            layoutGroup = cardContainerObject.AddComponent<HorizontalLayoutGroup>();
        }
        else
        {
            layoutGroup = cardContainerObject.AddComponent<VerticalLayoutGroup>();
        }
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = true;

        CardContainer cardContainer = cardContainerObject.AddComponent<CardContainer>();
        blackjackPlayer.cardContainer = cardContainer;

        return cardContainerObject;
    }

    public static Text InitTotalText(BlackjackPlayer blackjackPlayer)
    {
        string totalObjectName = GetObjectName(blackjackPlayer, "TotalText");
        GameObject totalObject = new GameObject(totalObjectName);
        totalObject.transform.parent = blackjackPlayer.cardContainer.transform;
        Text totalText = totalObject.ScaffoldTextComponent();
        return totalText;
    }

    public static GameObject InitButtonContainer(GameObject parentObject)
    {
        GameObject blackjackButtonContainer = new GameObject(BlackjackConstants.buttonContainerName);
        blackjackButtonContainer.transform.parent = parentObject.transform;
        blackjackButtonContainer.AddComponent<GridLayoutGroup>().cellSize = BlackjackConstants.buttonCellSize;
        return blackjackButtonContainer;
    }

    public static GameObject InitDrawnCardObject(BlackjackPlayer blackjackPlayer, Card drawnCard)
    {
        GameObject drawnCardObject = new GameObject("CardObject");
        drawnCardObject.transform.parent = blackjackPlayer.cardContainer.transform;
        drawnCardObject.AddComponent<Image>().sprite = !blackjackPlayer.IsDealer ? drawnCard.sprite : Deck.cardbackSprite;
        return drawnCardObject;
    }

    private static string GetObjectName(BlackjackPlayer blackjackPlayer, string text)
    {
        return $"{Enum.GetName(typeof(BlackjackPlayer), blackjackPlayer.type)}{text}";
    }
}