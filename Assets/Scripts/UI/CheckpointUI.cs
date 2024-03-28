using Events;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] private Button day2Button;
    //[SerializeField] private Transform buttonContainer;
    //[SerializeField] private GameObject buttonPrefab;
    //private readonly List<Button> buttons = new();

    private void Awake()
    {
        day2Button.AddOnClick(Day2Handler);

        //foreach (var checkpoint in CheckpointManager.Checkpoints)
        //{
        //    var instance = Instantiate(buttonPrefab, buttonContainer);
        //    instance.name = checkpoint.Name + " Button";
        //    var button = instance.GetComponent<Button>();
        //    button.AddOnClick(() => OnCheckpointClicked(checkpoint));
        //    button.SetText(checkpoint.Name);
        //    buttons.Add(button);
        //}

        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);
    }

    private void Day2Handler()
    {
        CheckpointManager.Day2();
        UIManager.ClearCanvases();
        day2Button.gameObject.SetActive(false);
    }

    //private void OnCheckpointClicked(Checkpoint checkpoint)
    //{
    //    checkpoint.Action();
    //    UIManager.ClearCanvases();
    //}

    private void OnMorningStartHandler()
    {
        if (CalendarManager.CurrentDate >= new Date(2, 1, 1))
        {
            day2Button.gameObject.SetActive(false);

            //var day2Button = buttons.FirstOrDefault(b => b.gameObject.name == "Go to Day 2 Button");
            //if (day2Button == null)
            //{
            //    Debug.LogWarning("Couldn't find day 2 checkpoint button to disable");
            //    return;
            //}
            //day2Button.gameObject.SetActive(false);
        }
    }
}
