using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private Text outcomeText;

    private void Awake()
    {
        stackButton = RepairsMinigamesManager
            .GetRepairsMinigameButton(RepairsMinigameButton.A);

        replayButton = RepairsMinigamesManager
            .GetRepairsMinigameButton(RepairsMinigameButton.B);
    }

    public void SetGameUI(GameState gameState)
    {
        outcomeText.SetText(GetGameOutcomeText(gameState));

        switch (gameState)
        {
            case GameState.NewGame:
                stackButton.SetActive(true);
                replayButton.SetActive(false);
                break;

            case GameState.Win:
            case GameState.Lose:
                stackButton.SetActive(false);
                replayButton.SetActive(true);
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
