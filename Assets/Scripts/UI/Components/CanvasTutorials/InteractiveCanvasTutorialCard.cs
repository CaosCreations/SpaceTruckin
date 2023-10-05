using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveCanvasTutorialCard : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Text tutorialText;

    [field: SerializeField]
    public GameObject[] SpotlightObjects { get; private set; }

    [field: SerializeField]
    public bool UnlockCanvas { get; private set; }

    public event UnityAction OnClosed;

    private void Awake()
    {
        if (closeButton != null && closeButton.gameObject.activeSelf)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseButtonHandler);
        }
        tutorialText.SetText(tutorialText.text);
    }

    // TODO: May need to be OnDisable? (If over tab buttons disable it indirectly)
    private void CloseButtonHandler()
    {
        OnClosed?.Invoke();
        SetSpotlightOjectsActive(false);
        gameObject.SetActive(false);
    }

    public void SetSpotlightOjectsActive(bool value)
    {
        if (SpotlightObjects == null)
        {
            return;
        }
        Array.ForEach(SpotlightObjects, s => s.SetActive(value));
    }
}