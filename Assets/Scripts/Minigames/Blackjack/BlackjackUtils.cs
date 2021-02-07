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

    public static GameObject InitTableContainer(GameObject parentObject)
    {
        GameObject blackjackTableContainer = new GameObject(BlackjackConstants.tableContainerName);
        blackjackTableContainer.transform.parent = parentObject.transform;

        blackjackTableContainer.AddComponent<LayoutElement>();
        VerticalLayoutGroup verticalLayoutGroup = blackjackTableContainer.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.childControlWidth = true;
        verticalLayoutGroup.childControlHeight = true;
        verticalLayoutGroup.childForceExpandWidth = true;
        verticalLayoutGroup.childForceExpandHeight = true;
        verticalLayoutGroup.spacing = BlackjackConstants.tableSpacing;
        verticalLayoutGroup.padding.left = BlackjackConstants.tablePaddingLeft;

        blackjackTableContainer.AddComponent<Image>().color = BlackjackConstants.tableColor;

        RectTransform rectTransform = blackjackTableContainer.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        return blackjackTableContainer;
    }

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

    public static GameObject InitCardContainer(GameObject parentObject, BlackjackPlayer blackjackPlayer)
    {
        string cardContainerName = GetObjectName(blackjackPlayer, "CardContainer");
        GameObject cardContainerObject = new GameObject(cardContainerName);
        cardContainerObject.transform.parent = parentObject.transform;

        CardContainer cardContainer = cardContainerObject.AddComponent<CardContainer>();
        blackjackPlayer.cardContainer = cardContainer;

        cardContainerObject.AddComponent<GridLayoutGroup>().cellSize = BlackjackConstants.cardCellSize;
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