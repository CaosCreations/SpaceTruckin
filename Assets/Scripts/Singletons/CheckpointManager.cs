using System.Linq;
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

        var convoIds = new int[] { 45, 51, 58, 73, 74, 75, 76, 77, 78, 79, 80, 163, 166, 167, 168, 171, 172, 175, 205 };
        DialogueDatabaseManager.Instance.SetConversationsSeen(convoIds);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, new Date(1, 1, 1), true);
        CalendarManager.SetDate(new Date(1, 1, 1));
        UIManager.LiftAccessSettings();

        // Destroy triggers that are for convos that we've auto-completed
        var cutsceneTriggers = FindObjectsOfType<CutsceneCollisionTrigger>();
        var triggerNames = new[] { "VestaBabyReceivingTrigger", "MeetVestaCleaningTrigger", "MeetPiratesTrigger", };
        foreach (var trigger in cutsceneTriggers)
        {
            if (triggerNames.Contains(trigger.name))
            {
                Destroy(trigger.gameObject);
            }
        }

        if (PlayerManager.PlayerMovementAnimation.IsHoldingBaby)
        {
            PlayerManager.PlayerMovementAnimation.ToggleBabyHolding();
        }

        // TODO: If we want to support going back to day 2 from a later day, we'll need to reset the dialogue db etc.
        Bed.Sleep();
    }
}
