using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObject_Player : MonoBehaviour, IAnimated
{
    // How do we get a reference to the Animator?
    [field: SerializeField] public Animator Animator { get; set; }

    public Dictionary<AnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<AnimationParameterType, string>()
    { 
        // Map the enum elements to the parameters 
        { AnimationParameterType.BatteryGrab, "batteryGrabbing" }
    };
}
