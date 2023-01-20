using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private Text outcomeText;

    public void SetGameUI(GameState gameState)
    {
        outcomeText.text = GetGameOutcomeText(gameState);

        switch (gameState)
        {
            case GameState.NewGame:
                stackButton.gameObject.SetActive(true);
                replayButton.gameObject.SetActive(false);
                break;

            case GameState.Win:
            case GameState.Lose:
                stackButton.gameObject.SetActive(false);
                replayButton.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    private string GetGameOutcomeText(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.NewGame:
                return string.Empty;

            case GameState.Win:
                return "You won!";

            case GameState.Lose:
                return "You lose!";

            default:
                return string.Empty;
        }
    }
}

