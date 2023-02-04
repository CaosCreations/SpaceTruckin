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

    private void CloseWindow()
    {
        UIManager.ClearCanvases();
    }
}