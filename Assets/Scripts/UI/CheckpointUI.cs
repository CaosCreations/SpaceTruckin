using Events;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] private Button day2Button;

    private void Awake()
    {
        day2Button.AddOnClick(Day2Handler);
        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);
    }

    private void Day2Handler()
    {
        CheckpointManager.Day2();
        UIManager.ClearCanvases();
        day2Button.gameObject.SetActive(false);
    }

    private void OnMorningStartHandler()
    {
        if (CalendarManager.CurrentDate >= new Date(2, 1, 1))
        {
            day2Button.gameObject.SetActive(false);
        }
    }
}
