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

        Button closeButton = GetComponentInChildren<Button>();
        
        if (closeButton != null)
        {
            closeButton.AddOnClick(CloseTutorial);
        }
    }

    private void SetTutorialText()
    {
        tutorialText.SetText(tutorialTextContent);
    }

    public void CloseTutorial()
    {
        UIManager.SetCurrentCanvasHasBeenViewed(true);
        Destroy(gameObject);
    }
}
