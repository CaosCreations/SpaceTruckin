using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlackjackUtils : MonoBehaviour
{
    public static void InitHeader(GameObject parentObject)
    {
        GameObject headerText = new GameObject(BlackjackConstants.headerName);
        headerText.transform.parent = parentObject.transform;
        Text text = headerText.ScaffoldTextComponent();
        text.text = BlackjackConstants.headerText;
    }

    public static Text InitGameInfo(GameObject parentObject, BlackjackPlayer blackjackPlayer)
    {
        GameObject gameInfoObject = new GameObject().ScaffoldUI(BlackjackConstants.gameInfoName, parentObject, BlackjackConstants.gameInfoAnchors);
        Text gameInfoText = gameInfoObject.ScaffoldTextComponent();
        gameInfoText.text = $"You have {blackjackPlayer.chips} chips. Place a bet to begin the game!";
        return gameInfoText;
    }

    public static GameObject InitCardContainer(GameObject parentObject, BlackjackPlayer blackjackPlayer, (Vector2, Vector2) anchors, bool isHorizontal)
    {
        GameObject cardContainerObject = new GameObject().ScaffoldUI("CardContainer", parentObject, anchors);

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
        GameObject totalText = new GameObject("TotalText");
        totalText.transform.parent = blackjackPlayer.cardContainer.transform;
        Text text = totalText.ScaffoldTextComponent();
        return text;
    }

    public static GameObject InitButtonContainer(GameObject parentObject)
    {
        GameObject blackjackButtonContainer = new GameObject().ScaffoldUI(
            BlackjackConstants.buttonContainerName, parentObject, BlackjackConstants.buttonContainerAnchors);
        
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
}