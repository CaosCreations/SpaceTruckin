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
        mainMenuButton.AddOnClick(MainMenuButtonHandler);
    }

    private void MainMenuButtonHandler()
    {
        creditsCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(true);
        SceneLoadingManager.Instance.LoadSceneAsync(SceneType.TitleScreen, loadingBarSlider);
    }
}
