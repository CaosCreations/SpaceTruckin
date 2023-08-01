using UnityEngine;

public class GameControls : MonoBehaviour
{
    [SerializeField] private StackMiniGame_GameplayManager gameplayManager;

    private void Update()
    {
        CheckSpaceBarInput();
    }

    private void CheckSpaceBarInput()
    {
        if (gameplayManager.GameStates.CheckCurrentState("new game") && Input.GetKeyDown(KeyCode.Space))
        {
            gameplayManager.StackCube();
        }
    }
}
