using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : UICanvasBase
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button quitGameButton;

    private void Awake()
    {
        newGameButton.AddOnClick(StartGame);
        quitGameButton.AddOnClick(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync(EnvironmentConstants.StationSceneName);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        UIManager.SetCannotInteract();
        UIManager.ResetOverriddenKeys();
    }
}
