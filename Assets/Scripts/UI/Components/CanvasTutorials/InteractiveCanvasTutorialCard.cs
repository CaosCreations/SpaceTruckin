using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveCanvasTutorialCard : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Text tutorialText;

    public event UnityAction OnClosed;

    private void Awake()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseButtonHandler);
        tutorialText.SetText(tutorialText.text);
    }

    private void CloseButtonHandler()
    {
        OnClosed?.Invoke();
        gameObject.SetActive(false);
    }
}