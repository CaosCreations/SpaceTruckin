using Events;
using UnityEngine;
using UnityEngine.UI;

public class TileWalkingUI : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private PlayerControls playerControls;

    [SerializeField] private Text gameOverText;

    [SerializeField] private Text winText;

    [SerializeField] private Button gameOverButton;

    private void Awake()
    {
        gameOverButton.onClick.RemoveAllListeners();
        gameOverButton.onClick.AddListener(gridManager.ResetGrid);
        gameOverButton.onClick.AddListener(playerControls.ResetPlayerMovement);
        gameOverButton.onClick.AddListener(DisableAllUIElements);

        SingletonManager.EventService.Add<OnRepairsMinigameLostEvent>(ToggleGameOverUI);
        SingletonManager.EventService.Add<OnRepairsMinigameWonEvent>(ToggleWinUI);
        SingletonManager.EventService.Add<OnRepairsToolBoughtEvent>(SetButtonInteractability);
    }

    public void ToggleGameOverUI(OnRepairsMinigameLostEvent lostEvent)
    {
        gameOverText.gameObject.SetActive(!gameOverText.gameObject.activeSelf);
        gameOverButton.gameObject.SetActive(!gameOverButton.gameObject.activeSelf);
        SetButtonInteractability();
    }

    public void ToggleWinUI(OnRepairsMinigameWonEvent wonEvent)
    {
        winText.gameObject.SetActive(!winText.gameObject.activeSelf);
        gameOverButton.gameObject.SetActive(!gameOverButton.gameObject.activeSelf);
        SetButtonInteractability();
    }

    private void DisableAllUIElements()
    {
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        gameOverButton.gameObject.SetActive(false);
    }

    //private void SetButtonInteractability(OnRepairsToolBoughtEvent boughtEvent)
    //{
    //    SetButtonInteractability();
    //}

    private void SetButtonInteractability()
    {
        gameOverButton.interactable = ShipsManager.CanRepair;
    }
}
