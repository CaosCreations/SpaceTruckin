using UnityEngine;

public abstract class InteractiveCanvasTutorial : SubMenu
{
    [SerializeField] protected InteractiveCanvasTutorialCard openingCard;

    public override void OnEnable()
    {
        base.OnEnable();
        openingCard.SetActive(true);
    }

    public override void OnDisable()
    {
    }

    protected virtual void EndTutorial()
    {
        // TODO: Set PlayerPrefs here instead of UIManager?
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
        gameObject.SetActive(false);
    }
}