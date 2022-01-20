using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : UICanvasBase
{
    [SerializeField] private Button MainMenuButton;

    private void Awake()
    {
        MainMenuButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.AddListener(ShowMainMenuCanvas);
    }

    private void ShowMainMenuCanvas()
    {
        UIManager.ShowCanvas(canvasType: UICanvasType.MainMenu, viaShortcut: false);
    }

}
