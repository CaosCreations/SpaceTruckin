using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] private Button day2Button;

    private void Awake()
    {
        day2Button.AddOnClick(Day2Handler);
    }

    private void Day2Handler()
    {
        CheckpointManager.Day2();
        UIManager.ClearCanvases();
    }
}
