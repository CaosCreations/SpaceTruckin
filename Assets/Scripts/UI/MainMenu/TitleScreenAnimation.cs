using UnityEngine;
using UnityEngine.UI;

public class TitleScreenAnimation : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Animator characterCreationAnimator;

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        playButton.onClick.AddListener(OnPlayButton);
        backButton.onClick.AddListener(OnBackButton);
    }

    private void OnPlayButton()
    {
        characterCreationAnimator.SetTrigger(AnimationConstants.OpenCharCreationTrigger);
    }

    private void OnBackButton()
    {
        characterCreationAnimator.SetTrigger(AnimationConstants.CloseCharCreationTrigger);
    }
}
