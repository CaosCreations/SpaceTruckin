using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : UICanvasBase
{
    public enum PauseMenuPanel
    {
        None = 0, Main = 1, Options = 2
    }

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button mainButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private Image playerImage;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Button mainMenuButton;

    private PauseMenuPanel currentPanel = PauseMenuPanel.None;

    private void Awake()
    {
        playerImage.sprite = PlayerManager.Instance.PlayerAnimatorSettings.Sprite;
        playerNameText.SetText(PlayerManager.Instance.PlayerName, FontType.Title);
        SwitchPanel(PauseMenuPanel.Main);

        mainButton.AddOnClick(() => SwitchPanel(PauseMenuPanel.Main));
        optionsButton.AddOnClick(() => SwitchPanel(PauseMenuPanel.Options));
        // Disabled for the demo
        //mainMenuButton.AddOnClick(GoToTitleScreen);
    }

    private void OnEnable()
    {
        SwitchPanel(PauseMenuPanel.Main);
    }

    private void SwitchPanel(PauseMenuPanel panel)
    {
        if (panel == currentPanel)
            return;

        ClearPanels();

        switch (panel)
        {
            case PauseMenuPanel.Main:
                mainPanel.SetActive(true);
                break;
            case PauseMenuPanel.Options:
                optionsPanel.SetActive(true);
                break;
        }
        currentPanel = panel;
    }

    private void ClearPanels()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    private void GoToTitleScreen()
    {
        if (SceneLoadingManager.Instance == null)
            throw new System.Exception("SceneLoadingManager object not found. Unable to go back to title screen.");

        UIManager.ClearCanvases();
        SceneLoadingManager.Instance.LoadScene(SceneType.TitleScreen);
    }
}
