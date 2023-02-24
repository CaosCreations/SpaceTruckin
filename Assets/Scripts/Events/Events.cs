using System.Linq;
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

    public class OnEventWithParameters : IEvent
    {
        public string Parameter { get; set; }

        public OnEventWithParameters(string parameter)
        {
            Parameter = parameter;
        }
    }
}
