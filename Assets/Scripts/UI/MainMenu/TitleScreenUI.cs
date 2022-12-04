using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    // Buttons
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmCharacterButton;

    // Main button canvas children
    [SerializeField] private GameObject mainButtonCanvas;
    [SerializeField] private GameObject mainButtonContainer;

    // Secondary canvases 
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject characterCreationCanvas;
    [SerializeField] private GameObject loadingScreenCanvas;

    [SerializeField] private GameObject stationBackground;
    private Slider loadingBarSlider;

    private void Start()
    {
        mainButtonCanvas.SetActive(true);
        mainButtonContainer.SetActive(true);
        backButton.SetActive(false);
        stationBackground.SetActive(true);
        loadingBarSlider = loadingScreenCanvas.GetComponentInChildren<Slider>();

        ResetSecondaryCanvases();
        AddListeners();
    }

    private void AddListeners()
    {
        playButton.AddOnClick(OnPlayButton);
        optionsButton.AddOnClick(OnOptionsButton);
        creditsButton.AddOnClick(OnCreditsButton);
        backButton.AddOnClick(OnBackButton);
        confirmCharacterButton.AddOnClick(OnConfirmButton);
    }

    private void OnPlayButton()
    {
        ResetSecondaryCanvases();
        characterCreationCanvas.SetActive(true);
        mainButtonContainer.SetActive(false);
        backButton.SetActive(true);
    }

    private void OnOptionsButton()
    {
        ResetSecondaryCanvases();
        optionsCanvas.SetActive(true);
        mainButtonContainer.SetActive(false);
        backButton.SetActive(true);
    }

    private void OnCreditsButton()
    {
        ResetSecondaryCanvases();
        creditsCanvas.SetActive(true);
        backButton.SetActive(true);
        mainButtonContainer.SetActive(false);
    }

    private void OnBackButton()
    {
        ResetSecondaryCanvases();
        mainButtonContainer.SetActive(true);
        backButton.SetActive(false);
    }

    private void OnConfirmButton()
    {
        if (SceneLoadingManager.Instance == null)
            throw new System.Exception("SceneLoadingManager object not found. Unable to confirm character selection");

        ResetSecondaryCanvases();
        backButton.SetActive(false);
        mainButtonCanvas.SetActive(false);
        stationBackground.SetActive(false);
        loadingScreenCanvas.SetActive(true);

        // Load scene after confirmation
        SceneLoadingManager.Instance.LoadSceneAsync(Scenes.MainStation, loadingBarSlider);
    }

    private void ResetSecondaryCanvases()
    {
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        characterCreationCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(false);
    }
}