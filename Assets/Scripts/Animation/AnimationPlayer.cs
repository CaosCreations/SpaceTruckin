using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer: MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private string AnimationName;

    public void PlayAnimation()
    {
        playerAnimator.SetBool(AnimationName, true);
    }
}
