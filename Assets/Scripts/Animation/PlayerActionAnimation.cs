using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionAnimation: MonoBehaviour
{
    public static PlayerActionAnimation Instance;

    [SerializeField] private Animator playerAnimator;

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

    public void PlayGrabBatteryAnimation()
    {
        playerAnimator.SetBool(AnimationConstants.AnimationBatteryGrabbingParameter, true);
    }
}
