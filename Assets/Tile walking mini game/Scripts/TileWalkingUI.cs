using System.Collections;
using System.Collections.Generic;
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

        gridManager.WinEvent += ToggleWinUI;
        gridManager.LoseEvent += ToggleGameOverUI;
    }

    public void ToggleGameOverUI()
    {
        gameOverText.gameObject.SetActive(!gameOverText.gameObject.activeSelf);
        gameOverButton.gameObject.SetActive(!gameOverButton.gameObject.activeSelf);
    }

    public void ToggleWinUI()
    {
        winText.gameObject.SetActive(!winText.gameObject.activeSelf);
        gameOverButton.gameObject.SetActive(!gameOverButton.gameObject.activeSelf);
    }

    private void DisableAllUIElements()
    {
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        gameOverButton.gameObject.SetActive(false);
    }
}
