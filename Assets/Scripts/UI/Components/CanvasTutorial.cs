using UnityEngine;
using UnityEngine.UI;

public class CanvasTutorial : MonoBehaviour
{
    [SerializeField] private Text tutorialText;
    [SerializeField] private string tutorialTextContent;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SetTutorialText();
        SetupCloseButton();
    }

    private void SetTutorialText()
    {
        tutorialText.SetText(tutorialTextContent);
    }

    private void SetupCloseButton()
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
