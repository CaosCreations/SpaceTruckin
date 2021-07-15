using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager Instance;

    [SerializeField] private Animator playerAnimator;

    public Dictionary<PlayerAnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<PlayerAnimationParameterType, string>()
    {
        { PlayerAnimationParameterType.BatteryGrab, "batteryGrabbing" },
    };

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

    public void PlayAnimation(PlayerAnimationParameterType playerAnimationParameterType, bool isOn)
    {
        if (ParameterMap.ContainsKey(playerAnimationParameterType))
        {
            playerAnimator.SetBool(ParameterMap[playerAnimationParameterType], isOn);
        }

        else
        {
            Debug.LogError("The playerAnimationParameterType is missing from the ParameterMap dictionary. Please add it.");
        }
    }
}
