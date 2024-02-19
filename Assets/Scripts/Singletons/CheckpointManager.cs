using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

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

    public static void Day2()
    {
        // TODO: Load station scene if not already

        MissionUtils.StartMissionByName("Alcohol for adults", "B. Lrorllyl");
        // TODO: May have to complete the convos corresponding to these jobs?
        MissionUtils.AcceptMissionByName("Vukra Aid");
        MissionUtils.AcceptMissionByName("Towels");

        var convoIds = new int[] { 166, 167, 171, 172, 163, 168, 205, 175, 51, 58, 45, };
        DialogueDatabaseManager.Instance.SetConversationsSeen(convoIds);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, new Date(1, 1, 1), true);
        CalendarManager.SetDate(new Date(1, 1, 1));
        UIManager.LiftAccessSettings();

        // TODO: If we want to support going back to day 2 from a later day, we'll need to reset the dialogue db etc.
        Bed.Sleep();
    }
}
