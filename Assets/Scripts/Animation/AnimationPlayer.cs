using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer: MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private string animationName;

    [SerializeField] private bool play;

    public void PlayAnimation()
    {
        Debug.Log("Playing " + animationName + " animation");
        playerAnimator.SetBool(animationName, play);
    }
}
