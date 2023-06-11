using Events;
using UnityEngine;

public class SleepActionManager : MonoBehaviour
{
    public static SleepActionManager Instance { get; private set; }

    [SerializeField]
    private SleepActionContainer actionContainer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);
    }

    private void OnMorningStartHandler()
    {
        Debug.Log("Checking for sleep actions...");
        foreach (var action in actionContainer.Elements)
        {
            // OnMorningStart uses the Wake phase 
            if (action.SleepPhase != SleepAction.Phase.Wake || action.Date != CalendarManager.CurrentDate)
            {
                continue;
            }

            Debug.Log("Executing " + action);
            action.Execute();
        }
    }
}