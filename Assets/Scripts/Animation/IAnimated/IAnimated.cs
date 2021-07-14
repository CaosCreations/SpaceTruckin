using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimated
{
    [SerializeField] public Animator Animator { get; set; }

    public Dictionary<AnimationParameterType, string> ParameterMap { get; set; }
}
