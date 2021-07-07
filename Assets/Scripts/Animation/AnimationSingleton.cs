using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSingleton : MonoBehaviour
{
    public static AnimationSingleton Instance;

    public Animator playerAnimator;

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

    public void PlayAnimation(Animator animator, string animationParameter, bool play)
    {
        animator.SetBool(animationParameter, play);
    }
}
