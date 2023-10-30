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
    [SerializeField] private Cutscene babyHoldStartCutscene;
    [SerializeField] private Cutscene babyHoldStopCutscene;
    public bool IsHoldingBaby { get; private set; }

    [SerializeField]
    private bool manualSwitchBabyMode;

    private void Awake()
    {
        parameterMap = AnimationConstants.PlayerMovementAnimationMap;
        IsHoldingBaby = false;
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    public void SetParams()
    {
        ResetParams();
        animator.SetBool(AnimationConstants.AnimationRunParameter, PlayerManager.PlayerMovement.IsRunning);

        if (parameterMap.ContainsKey(PlayerMovement.MovementVector))
        {
            string[] activeParams = parameterMap[PlayerMovement.MovementVector];
            Array.ForEach(activeParams, p => animator.SetBool(p, true));
        }
    }

    public void ResetParams()
    {
        animator.SetBool(AnimationConstants.AnimationUpParameter, false);
        animator.SetBool(AnimationConstants.AnimationLeftParameter, false);
        animator.SetBool(AnimationConstants.AnimationDownParameter, false);
        animator.SetBool(AnimationConstants.AnimationRightParameter, false);
    }

    public void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (evt.Cutscene == babyHoldStartCutscene)
        {
            SwitchBabyHolding(true);
        }
        else if (evt.Cutscene == babyHoldStopCutscene)
        {
            SwitchBabyHolding(false);
        }
    }

    public void ToggleBabyHolding()
    {
        SwitchBabyHolding(!IsHoldingBaby);
    }

    private void SwitchBabyHolding(bool isHoldingBaby)
    {
        animator.Play("StandUpP");
        IsHoldingBaby = isHoldingBaby;
        Debug.Log("Baby holding switched to: " + isHoldingBaby);
    }

    public void UpdateIdle()
    {
        string stateName = default;
        if (PlayerMovement.PlayerFacingDirection == new Vector3(-1, 0, 0))
        {
            stateName = AnimationConstants.PlayerIdleLeftParameter;
        }
        else if (PlayerMovement.PlayerFacingDirection == new Vector3(1, 0, 0))
        {
            stateName = AnimationConstants.PlayerIdleRightParameter;
        }
        else if (PlayerMovement.PlayerFacingDirection == new Vector3(0, 0, 1))
        {
            stateName = AnimationConstants.PlayerIdleUpParameter;
        }
        else if (PlayerMovement.PlayerFacingDirection == new Vector3(0, 0, -1))
        {
            stateName = AnimationConstants.PlayerIdleDownParameter;
        }

        if (stateName != null)
        {
            animator.Play(stateName);
        }
    }

    private void Update()
    {
        if (manualSwitchBabyMode)
        {
            ToggleBabyHolding();
            manualSwitchBabyMode = false;
        }
    }
}
