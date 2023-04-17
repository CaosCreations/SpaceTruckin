﻿using System.Linq;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Events
{
    public class OnSceneChangeEvent : IEvent
    {
        public Scene Scene { get; set; }
        public bool IsRepairsMinigameScene { get; }

        public OnSceneChangeEvent(Scene scene)
        {
            Scene = scene;

            var sceneType = scene.GetSceneType();
            IsRepairsMinigameScene = RepairsConstants.RepairsMinigameSceneTypes.Contains(sceneType);
        }
    }

    public class OnSceneLoadedEvent : OnSceneChangeEvent
    {
        public OnSceneLoadedEvent(Scene scene) : base(scene)
        {
        }
    }

    public class OnSceneUnloadedEvent : OnSceneChangeEvent
    {
        public OnSceneUnloadedEvent(Scene scene) : base(scene)
        {
        }
    }

    public abstract class OnCutsceneEvent : IEvent
    {
        public Cutscene Cutscene { get; }

        public OnCutsceneEvent(Cutscene cutscene)
        {
            Cutscene = cutscene;
        }
    }

    public class OnCutsceneStartedEvent : OnCutsceneEvent
    {
        public OnCutsceneStartedEvent(Cutscene cutscene) : base(cutscene)
        {
        }
    }

    public class OnCutsceneFinishedEvent : OnCutsceneEvent
    {
        public OnCutsceneFinishedEvent(Cutscene cutscene) : base(cutscene)
        {
        }
    }

    public abstract class OnTimelineEvent : IEvent
    {
        public PlayableAsset PlayableAsset { get; }

        public OnTimelineEvent(PlayableAsset playableAsset)
        {
            PlayableAsset = playableAsset;
        }
    }

    public class OnTimelineStartedEvent : OnTimelineEvent
    {
        public OnTimelineStartedEvent(PlayableAsset playableAsset) : base(playableAsset)
        {
        }
    }

    public class OnTimelineFinishedEvent : OnTimelineEvent
    {
        public OnTimelineFinishedEvent(PlayableAsset playableAsset) : base(playableAsset)
        {
        }
    }

    public class OnHangarNodeTerminalClosedEvent : IEvent
    {
    }

    public class OnHangarNodeTerminalOpenedEvent : IEvent
    {
        public Ship Ship { get; set; }

        public OnHangarNodeTerminalOpenedEvent(Ship ship)
        {
            Ship = ship;
        }
    }

    public class OnShipLaunchedEvent : IEvent
    {
    }

    public class OnRepairsMinigameStartedEvent : IEvent
    {
    }

    public abstract class OnRepairsMinigameOutcomeEvent : IEvent
    {
        public RepairsMinigameType MinigameType { get; }

        public OnRepairsMinigameOutcomeEvent(RepairsMinigameType minigameType)
        {
            MinigameType = minigameType;
        }
    }

    public class OnRepairsMinigameWonEvent : OnRepairsMinigameOutcomeEvent
    {
        public OnRepairsMinigameWonEvent(RepairsMinigameType minigameType) : base(minigameType)
        {
        }
    }

    public class OnRepairsMinigameLostEvent : OnRepairsMinigameOutcomeEvent
    {
        public OnRepairsMinigameLostEvent(RepairsMinigameType minigameType) : base(minigameType)
        {
        }
    }

    public class OnRepairsToolBoughtEvent : IEvent
    {
    }

    public class OnRepairsToolSpentEvent : IEvent
    {
    }

    public class OnShipHealthChangedEvent : IEvent
    {
        public Ship Ship { get; }

        public OnShipHealthChangedEvent(Ship ship)
        {
            Ship = ship;
        }
    }

    public class OnModifierReportCardOpenedEvent : IEvent
    {
    }

    public class OnModifierReportCardClosedEvent : IEvent
    {
    }

    public class OnEndOfDayEvent : IEvent
    {
    }

    public class OnEveningStartEvent : IEvent
    {
    }

    public class OnLightsOutTimeEvent : IEvent
    {
    }

    public class OnClockStoppedEvent : IEvent
    {
    }

    public class OnClockStartedEvent : IEvent
    {
    }

    public class OnPlayerSleepEvent : IEvent
    {
    }

    public class OnPlayerPausedEvent : IEvent
    {
    }

    public class OnPlayerUnpausedEvent : IEvent
    {
    }

    public class OnCameraZoomInEndedEvent : IEvent
    {
        public StationCamera.Identifier Identifier { get; }

        public OnCameraZoomInEndedEvent(StationCamera.Identifier identifier)
        {
            Identifier = identifier;
        }
    }

    public class OnLiveCameraZoomInEndedEvent : IEvent
    {
    }

    public class OnMissionSlottedEvent : IEvent
    {
    }

    public class OnPilotSlottedEvent : IEvent
    {
    }

    public class OnBatteryChargedEvent : IEvent
    {
        public StringBoolKeyValueEventArgs Args { get; }

        public OnBatteryChargedEvent(StringBoolKeyValueEventArgs eventArgs)
        {
            Args = eventArgs;
        }
    }
}
