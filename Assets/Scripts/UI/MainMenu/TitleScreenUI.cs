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
    [SerializeField] private GameObject characterCreationCanvas;

    private void Start()
    {
        mainButtonContainer.SetActive(true);
        characterCreationCanvas.SetActive(false);
        backButton.SetActive(false);
        AddListeners();
    }

    private void AddListeners()
    {
        playButton.AddOnClick(OnPlayButton);
        optionsButton.AddOnClick(OnOptionsButton);
        creditsButton.AddOnClick(OnCreditsButton);
        backButton.AddOnClick(OnBackButton);
    }

    private void OnPlayButton()
    {
        mainButtonContainer.SetActive(false);
        characterCreationCanvas.SetActive(true);
        backButton.SetActive(true);
    }

    private void OnOptionsButton()
    {

    }

    private void OnCreditsButton()
    {

    }

    private void OnBackButton()
    {
        mainButtonContainer.SetActive(true);
        characterCreationCanvas.SetActive(false);
        backButton.SetActive(false);
    }
}