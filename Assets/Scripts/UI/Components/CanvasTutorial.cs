using UnityEngine;

public class CanvasTutorial : SubMenu
{
    [SerializeField] private CardCycle tutorialCardCycle;

    public override void OnDisable()
    {
        UIManager.SetCurrentCanvasHasBeenViewed(true);
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
        Destroy(gameObject);
    }
}
