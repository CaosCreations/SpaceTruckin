using Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Dictionary<Vector3, string[]> parameterMap;

    [Header("Cutscene settings")]
    [SerializeField] private string babyStartCutsceneName;
    [SerializeField] private string babyStopCutsceneName;
    public bool BabyMode => parameterMap == AnimationConstants.BabyMovementAnimationMap;

    private void Awake()
    {
        parameterMap = AnimationConstants.PlayerMovementAnimationMap;
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
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
        //animator.SetBool(AnimationConstants.BabyUpParameter, false);
        //animator.SetBool(AnimationConstants.BabyLeftParameter, false);
        //animator.SetBool(AnimationConstants.BabyDownParameter, false);
        //animator.SetBool(AnimationConstants.BabyRightParameter, false);
    }

    public void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (parameterMap == AnimationConstants.PlayerMovementAnimationMap && evt.Cutscene.Name == babyStartCutsceneName)
        {
            parameterMap = AnimationConstants.BabyMovementAnimationMap;
        }
        else if (parameterMap == AnimationConstants.BabyMovementAnimationMap && evt.Cutscene.Name == babyStopCutsceneName)
        {
            parameterMap = AnimationConstants.PlayerMovementAnimationMap;
        }
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
