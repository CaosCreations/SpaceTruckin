using UnityEngine;

public abstract class InteractiveCanvasTutorial : CanvasTutorial
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
}