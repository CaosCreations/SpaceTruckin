using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // To call the function, select a variable in the AnimationConstants.cs file and pass it as animationParameter 
    public void ChangeAnimation(string animationParameter, bool isOn)
    {
        animator.SetBool(animationParameter, isOn);
    }
}
