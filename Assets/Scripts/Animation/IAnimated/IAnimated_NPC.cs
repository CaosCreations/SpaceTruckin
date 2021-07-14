using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAnimated_NPC : MonoBehaviour, IAnimated
{
    // How do we get a reference to the Animator?
    [SerializeField] public Animator Animator { get; set; }
    public Dictionary<AnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<AnimationParameterType, string>()
    {
        { AnimationParameterType.Idle, "Idle" },
        { AnimationParameterType.Walking, "Walking" },
    };
}
