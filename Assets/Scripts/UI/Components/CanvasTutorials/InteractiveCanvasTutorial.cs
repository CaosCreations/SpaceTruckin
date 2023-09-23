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
    [SerializeField] protected string dialogueBoolOnComplete;

    protected override void OnEnable()
    {
        if (lockCanvas)
        {
            base.OnEnable();
        }
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
        universalUI.DisableCloseWindowButton();
        if (cantExitCard != null)
        {
            universalUI.AddCloseWindowButtonListener(CloseWindowButtonHandler);
        }
        else
        {
            Debug.LogWarning("\"Can't exit\" card for locking canvas does not exist");
        }
    }

    protected void UnlockCanvas()
    {
        universalUI.RemoveCloseWindowButtonListener(CloseWindowButtonHandler);
        universalUI.EnableCloseWindowButton();
        RemoveOverriddenKeys();
        lockCanvas = false;
    }

    protected virtual void EndingCardHandler()
    {
        EndTutorial();
    }

    protected virtual void EndTutorial()
    {
        if (lockCanvas)
        {
            UnlockCanvas();
        }
        DialogueDatabaseManager.Instance.UpdateDatabaseVariable(dialogueBoolOnComplete, true);
        RemoveOverriddenKeys();
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
        if (card.UnlockCanvas)
        {
            UnlockCanvas();
        }
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