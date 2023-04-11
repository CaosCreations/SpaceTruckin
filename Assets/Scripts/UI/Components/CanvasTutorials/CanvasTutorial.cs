using UnityEngine;

public class CanvasTutorial : SubMenu
{
    [SerializeField] private CardCycle tutorialCardCycle;

    protected virtual void EndTutorial()
    {
        UIManager.SetCurrentCanvasHasBeenViewed(true);
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
        Destroy(gameObject);
    }

    public override void OnDisable()
    {
        EndTutorial();
    }
}
