using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmCharacterButton;

    [SerializeField] private GameObject mainButtonCanvas;
    [SerializeField] private GameObject mainButtonContainer;

    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject creditsCanvas;

    [SerializeField] private GameObject characterCreationCanvas;
    [SerializeField] private GameObject characterCreationContainer;

    private void Awake()
    {
        // Buttons with multiple listeners
        playButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        mainButtonCanvas.SetActive(true);
        mainButtonContainer.SetActive(true);
        backButton.SetActive(false);
        ResetSecondaryCanvases();
        AddListeners();
    }

    private void AddListeners()
    {
        // Buttons with multiple listeners
        playButton.onClick.AddListener(OnPlayButton);
        backButton.onClick.AddListener(OnBackButton);

        optionsButton.AddOnClick(OnOptionsButton);
        creditsButton.AddOnClick(OnCreditsButton);
        backButton.AddOnClick(OnBackButton);
        confirmCharacterButton.AddOnClick(OnConfirmButton);
    }

    private void OnPlayButton()
    {
        ResetSecondaryCanvases();
        characterCreationContainer.SetActive(true);
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

        SceneLoadingManager.LoadScene(Scenes.MainStation);
    }

    private void ResetSecondaryCanvases()
    {
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        //characterCreationContainer.SetActive(false);
        characterCreationContainer.SetActive(false);
    }
}