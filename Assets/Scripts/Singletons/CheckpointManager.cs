using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public static Checkpoint[] Checkpoints { get; private set; } = new[]
    {
        new Checkpoint("Go to Day 2", Day2),
        new Checkpoint("Pirates Convo 1", PiratesConvo1)
    };

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

    public static void PiratesConvo1()
    {
        var convoIds = new int[] { 166, 167 };
        DialogueDatabaseManager.Instance.SetConversationsSeen(convoIds);
        PlayerManager.PlayerMovement.SetPosition(PlayerConstants.PlayerSpaceportPosition, AnimationConstants.SpaceportStateName);
    }

    public static void Day2()
    {
        MissionUtils.StartMissionByName("Alcohol for adults", "B. Lrorllyl");
        MissionUtils.AcceptMissionByName("Vukra Aid");
        MissionUtils.AcceptMissionByName("Towels");
        MissionUtils.AcceptMissionByName("Replacement Shipment");
        MissionUtils.AcceptMissionByName("ULSS Samples");

        var convoIds = new int[] { 45, 51, 58, 65, 73, 74, 75, 76, 77, 78, 79, 80, 163, 166, 167, 168, 171, 172, 175, 205 };
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

public class Checkpoint
{
    public string Name { get; private set; }
    public UnityAction Action { get; private set; }

    public Checkpoint(string name, UnityAction action)
    {
        Name = name;
        Action = action;
    }
}
