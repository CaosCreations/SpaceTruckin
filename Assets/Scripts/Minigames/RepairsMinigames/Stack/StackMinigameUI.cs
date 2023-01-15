using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Text outcomeText;

    private void Start()
    {
        //RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, true);
    }

    public void SetGameUI(MinigameState gameState)
    {
        string outcomeTextContent = GetGameOutcomeText(gameState);
        outcomeText.SetText(outcomeTextContent);
        //RepairsMinigameUIManager.Instance.SetOutcomeText(outcomeText);

        switch (gameState)
        {
            case MinigameState.NewGame:
                stackButton.SetActive(true);
                replayButton.SetActive(false);
                //RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, true);
                //RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.B, false);
                break;

            case MinigameState.Win:
            case MinigameState.Lose:
                stackButton.SetActive(false);
                replayButton.SetActive(true);
                //RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.A, false);
                //RepairsMinigameUIManager.Instance.SetButtonActive(RepairsMinigameButtonType.B, true);
                break;

            default:
                break;
        }
    }

    private string GetGameOutcomeText(MinigameState gameState)
    {
        switch (gameState)
        {
            case MinigameState.NewGame:
                return string.Empty;

            case MinigameState.Win:
                return "You won!";

            case MinigameState.Lose:
                return "You lose!";

            default:
                return string.Empty;
        }
    }
}
