using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    public void PlayAnimation(IAnimated animatedObject, AnimationParameterType animationParameterType, bool isOn)
    {
        if (animatedObject.ParameterMap.ContainsKey(animationParameterType))
        {
            animatedObject.Animator.SetBool(animatedObject.ParameterMap[animationParameterType], isOn);
        }

        else
        {
            Debug.LogError("The animationParameterType is missing from the ParameterMap dictionary of the animatedObject. Please add it.");
        }
    }
}
