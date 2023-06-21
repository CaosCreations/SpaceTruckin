using UnityEngine;

public class GeneralTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    // TODO: Cards 
    [SerializeField] private InteractiveCanvasTutorialCard card2;
    [SerializeField] private InteractiveCanvasTutorialCard card3;

    private bool card2Shown;
    private bool card3Shown;

    private void Start()
    {
        card3.OnClosed += EndTutorial;
    }

    private void CloseAllCards()
    {
        openingCard.SetActive(false);
        card2.SetActive(false);
        card3.SetActive(false);
    }
}
