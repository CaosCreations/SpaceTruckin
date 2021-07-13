using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator Animator;

    [SerializeField] private AnimationData[] animationData;

    // Index the Dictionary with the enum value, so no human error. 
    public Dictionary<AnimationType, string> ParameterMap;

    private void Awake()
    {
        ParameterMap = new Dictionary<AnimationType, string>();

        foreach (AnimationData item in animationData)
        {
            ParameterMap.Add(item.animationType, item.AnimationParameter);
        }
    }

    
}


