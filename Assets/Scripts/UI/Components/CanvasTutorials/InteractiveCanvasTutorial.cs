using UnityEngine;

public abstract class InteractiveCanvasTutorial : SubMenu
{
    [SerializeField] protected InteractiveCanvasTutorialCard openingCard;
    [SerializeField] protected bool lockCanvas;
    [SerializeField] protected InteractiveCanvasTutorialCard cantExitCard;
    [SerializeField] protected UniversalUI universalUI;

    public override void OnEnable()
    {
        base.OnEnable();
        openingCard.SetActive(true);
        if (lockCanvas)
        {
            LockCanvas();
        }
    }

    protected void LockCanvas()
    {
        universalUI.DisableCloseWindowButton();
        universalUI.AddCloseWindowButtonListener(CloseWindowButtonHandler);
    }

    protected void UnlockCanvas()
    {
        universalUI.RemoveCloseWindowButtonListener(CloseWindowButtonHandler);
        universalUI.EnableCloseWindowButton();
        lockCanvas = false;
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
}