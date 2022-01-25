using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : UICanvasBase
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.AddOnClick(ShowMainMenuCanvas);
    }

    private void ShowMainMenuCanvas()
    {
        UIManager.ShowCanvas(canvasType: UICanvasType.MainMenu, viaShortcut: false);
    }

}
