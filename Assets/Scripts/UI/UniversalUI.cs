using UnityEngine;
using UnityEngine.UI;

public class UniversalUI : MonoBehaviour
{
    [SerializeField]
    private Button closeWindowButton;

    private void Start()
    {
        closeWindowButton.AddOnClick(CloseWindow);
    }

    public void DisableCloseWindowButton()
    {

    }

    public void EnableCloseWindowButton()
    {

    }

    private void CloseWindow()
    {
        UIManager.ClearCanvases();
    }
}