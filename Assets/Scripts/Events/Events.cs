﻿using System.Linq;
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

    public class OnEventWithParameters : IEvent
    {
        public string Parameter { get; set; }

        public OnEventWithParameters(string parameter)
        {
            Parameter = parameter;
        }
    }

    public class OnRepairsMinigameSceneLoadedEvent : IEvent
    {
        public RepairsMinigameType RepairsMinigameType { get; private set; }

        public OnRepairsMinigameSceneLoadedEvent(RepairsMinigameType repairsMinigameType)
        {
            RepairsMinigameType = repairsMinigameType;
        }
    }
}