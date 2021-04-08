using UnityEngine;
using UnityEngine.UI;

public class CanvasTutorial : MonoBehaviour
{
    private void Start()
    {
        Button closeButton = GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.AddOnClick(CloseTutorial);
        }
    }

    public void CloseTutorial()
    {
        UIManager.SetCurrentCanvasHasBeenViewed(true);
        Destroy(gameObject);
    }
}