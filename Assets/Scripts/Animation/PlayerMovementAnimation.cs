using Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Dictionary<Vector3, string[]> parameterMap;

    private void Awake()
    {
        parameterMap = AnimationConstants.PlayerMovementAnimationMap;
    }

    public void SetDirection()
    {
        ResetDirection();

        if (parameterMap.ContainsKey(PlayerMovement.MovementVector))
        {
            // Get the matching parameters for the player's current direction  
            string[] activeParams = parameterMap[PlayerMovement.MovementVector];

            // Update the state machine 
            Array.ForEach(activeParams, p => animator.SetBool(p, true));
        }
    }

    public void ResetDirection()
    {
        animator.SetBool(AnimationConstants.AnimationUpParameter, false);
        animator.SetBool(AnimationConstants.AnimationLeftParameter, false);
        animator.SetBool(AnimationConstants.AnimationDownParameter, false);
        animator.SetBool(AnimationConstants.AnimationRightParameter, false);
    }

    public void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        // TODO: Could be more than one cutscene 
        //paramMap = evt.Cutscene.Name == "OpeningCutsceneSplinted" ? AnimationConstants.BabyMovementAnimationMap : AnimationConstants.PlayerMovementAnimationMap;
    }

    private void FixedUpdate()
    {
        if (parameterMap == AnimationConstants.PlayerMovementAnimationMap && Input.GetKey(PlayerConstants.SprintKey))
        {
            animator.SetBool(AnimationConstants.AnimationRunParameter, true);
        }
        else
        {
            animator.SetBool(AnimationConstants.AnimationRunParameter, false);
        }
    }
}
