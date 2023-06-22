using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UniversalUI : MonoBehaviour
{
    [SerializeField]
    private Button closeWindowButton;

    private void Start()
    {
        EnableCloseWindowButton();
    }

    public void AddCloseWindowButtonListener(UnityAction action)
    {
        closeWindowButton.AddOnClick(action, removeListeners: false);
    }

    public void RemoveCloseWindowButtonListener(UnityAction action)
    {
        closeWindowButton.onClick.RemoveListener(action);
    }

    /// <summary>
    /// Deliberately removes listener rather than greying out the button.
    /// </summary>
    public void DisableCloseWindowButton()
    {
        RemoveCloseWindowButtonListener(CloseWindow);
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