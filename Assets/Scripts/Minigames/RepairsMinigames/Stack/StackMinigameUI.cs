using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private StackMiniGame_GameplayManager gameplayManager;

    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private Text outcomeText;

    [SerializeField] private GameResultUIData[] gameResultMessages;

    private Dictionary<string, GameResultUIData> gameResultMessageDictionary = new Dictionary<string, GameResultUIData>();

    private void Awake()
    {
        foreach(GameResultUIData gameResultUIdata in gameResultMessages)
        {
            gameResultMessageDictionary.Add(gameResultUIdata.State, gameResultUIdata);
        }

        stackButton.onClick.RemoveAllListeners();
        stackButton.onClick.AddListener(() => gameplayManager.StackCube());

        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(gameplayManager.ResetGame);

        gameplayManager.GameResetEvent += () => SetGameUI(gameplayManager.GameStates);
        gameplayManager.GameEndEvent += () => SetGameUI(gameplayManager.GameStates);
    }

    public void SetGameUI(GameState gameState)
    {
        string result;

        foreach (GameResultUIData item in gameResultMessages) 
        { 
            if(gameState.CheckCurrentState(item.State) == true)
            {
                outcomeText.text = item.Message;

                stackButton.gameObject.SetActive(item.StackButtonActive);
                replayButton.gameObject.SetActive(item.ReplayButtonActive);

                break;
            }
        }
    }
}

