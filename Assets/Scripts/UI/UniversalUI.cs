using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UniversalUI : MonoBehaviour
{
    [SerializeField]
    private Button closeWindowButton;

    private void Awake()
    {
        EnableCloseWindowButton();
    }

    public void AddCloseWindowButtonListener(UnityAction action)
    {
        closeWindowButton.AddOnClick(action);
    }

    public void RemoveCloseWindowButtonListener(UnityAction action)
    {
        closeWindowButton.onClick.RemoveListener(action);
    }

    /// <summary>
    /// Deliberately removes listeners rather than greying out the button.
    /// </summary>
    public void DisableCloseWindowButton()
    {
        closeWindowButton.onClick.RemoveAllListeners();
    }

    public void EnableCloseWindowButton()
    {
        AddCloseWindowButtonListener(CloseWindow);
    }

    private void CloseWindow()
    {
        UIManager.ClearCanvases();
    }
}