using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class InteractiveCanvasTutorial : SubMenu
{
    [SerializeField] protected InteractiveCanvasTutorialCard openingCard;
    [SerializeField] protected InteractiveCanvasTutorialCard endingCard;
    [SerializeField] protected bool lockCanvas;
    [SerializeField] protected InteractiveCanvasTutorialCard cantExitCard;
    [SerializeField] protected UniversalUI universalUI;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    protected virtual void Start()
    {
        openingCard.SetActive(true);
        
        // Optional as some tutorials end via listening for external events or a combination of shown flags
        if (endingCard != null)
        {
            endingCard.OnClosed += EndingCardHandler;
        }

        if (lockCanvas)
        {
            LockCanvas();
        }
    }

    protected void LockCanvas()
    {
        if (cantExitCard == null)
        {
            Debug.LogError("Card for locking canvas does not exist");
            return;
        }
        universalUI.DisableCloseWindowButton();
        universalUI.AddCloseWindowButtonListener(CloseWindowButtonHandler);
    }

    protected void UnlockCanvas()
    {
        universalUI.RemoveCloseWindowButtonListener(CloseWindowButtonHandler);
        universalUI.EnableCloseWindowButton();
        lockCanvas = false;
    }

    protected virtual void EndingCardHandler()
    {
        EndTutorial();
    }

    public override void OnDisable()
    {
    }

    protected virtual void EndTutorial()
    {
        if (lockCanvas)
        {
            UnlockCanvas();
        }
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
        Destroy(gameObject);
    }

    protected void CloseWindowButtonHandler()
    {
        ShowCard(cantExitCard);
    }

    protected abstract void CloseAllCards();
    
    protected virtual void ShowCard(InteractiveCanvasTutorialCard card)
    {
        CloseAllCards();
        card.SetActive(true);
    }

    protected virtual void ShowCard(InteractiveCanvasTutorialCard card, ref bool cardShown, Button button, UnityAction buttonHandler)
    {
        if (cardShown)
            return;

        ShowCard(card);
        cardShown = true;
        button.onClick.RemoveListener(buttonHandler);
    }
}