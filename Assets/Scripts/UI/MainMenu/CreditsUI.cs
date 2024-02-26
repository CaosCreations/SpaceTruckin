using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private GameObject creditsCanvas;

    [SerializeField]
    private GameObject loadingScreenCanvas;

    private Slider loadingBarSlider;

    private void Awake()
    {
        loadingScreenCanvas.SetActive(false);
        loadingBarSlider = loadingScreenCanvas.GetComponentInChildren<Slider>();

        // If it's end of game, then don't let them return to main menu 
        if (CalendarManager.Instance != null && CalendarManager.IsEndOfCalendar)
        {
            mainMenuButton.SetActive(false);
        }
        else
        {
            mainMenuButton.SetActive(true);
            mainMenuButton.AddOnClick(MainMenuButtonHandler);
        }
    }

    private void MainMenuButtonHandler()
    {
        creditsCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(true);
        SceneLoadingManager.Instance.LoadSceneAsync(SceneType.TitleScreen, loadingBarSlider);
    }
}
