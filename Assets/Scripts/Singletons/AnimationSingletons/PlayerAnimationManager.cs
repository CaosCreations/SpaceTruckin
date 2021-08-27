using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : AnimationManager<PlayerAnimationParameterType>
{
    public static PlayerAnimationManager Instance;

    [SerializeField] private Animator playerAnimator;

    private Dictionary<PlayerAnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<PlayerAnimationParameterType, string>()
    {
        { PlayerAnimationParameterType.BatteryGrab, AnimationConstants.AnimationBatteryGrabbingParameter },
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
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
            LogMissingParameterMapping(playerAnimationParameterType);
        }
    }
}
