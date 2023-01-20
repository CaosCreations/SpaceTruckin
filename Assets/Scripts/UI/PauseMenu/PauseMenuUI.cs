using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : UICanvasBase
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.AddOnClick(GoToTitleScreen);
    }

    private void GoToTitleScreen()
    {
        if (SceneLoadingManager.Instance == null)
            throw new System.Exception("SceneLoadingManager object not found. Unable to go back to title screen.");

        UIManager.ClearCanvases();
        SceneLoadingManager.Instance.LoadScene(Scenes.TitleScreen);
    }
}
