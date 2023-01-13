using UnityEngine;

public class StackMinigameUI : MonoBehaviour
{
    private void Start()
    {
        RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, true);
    }

    public void SetGameUI(GameState gameState)
    {
        string outcomeText = GetGameOutcomeText(gameState);
        RepairsMinigameUIManager.Instance.SetOutcomeText(outcomeText);

        switch (gameState)
        {
            case GameState.NewGame:
                RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, true);
                RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.B, false);
                break;

            case GameState.Win:
            case GameState.Lose:
                RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, false);
                RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.B, true);
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
