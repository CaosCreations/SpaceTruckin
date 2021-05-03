using UnityEngine;

public class CanvasTutorial : MonoBehaviour
{
    [SerializeField] private CardCycle tutorialCardCycle;

    private void OnDisable()
    {
        UIManager.SetCurrentCanvasHasBeenViewed(true);
        Destroy(gameObject);
    }
}
