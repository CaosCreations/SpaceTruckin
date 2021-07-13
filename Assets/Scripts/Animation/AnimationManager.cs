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

    public void PlayAnimation(Animation animation, AnimationType animationType, bool isOn)
    {
        animation.Animator.SetBool(animation.ParameterMap[animationType], isOn);
    }
}
