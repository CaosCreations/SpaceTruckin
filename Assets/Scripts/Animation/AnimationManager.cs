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

    public void PlayAnimation(AnimatorData animatorData, AnimationParemeterType animationParemeterType, bool isOn)
    {
        animatorData.Animator.SetBool(animatorData.ParameterMap[animationParemeterType], isOn);
    }
}
