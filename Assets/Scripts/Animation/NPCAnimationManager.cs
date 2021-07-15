using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    public static NPCAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(this);
        }
    }

    public void PlayAnimation(INPCAnimated InpcAnimated ,NPCAnimationParameterType npcAnimationParameterType, bool isOn)
    {
        if (InpcAnimated.ParameterMap.ContainsKey(npcAnimationParameterType))
        {
            InpcAnimated.Animator.SetBool(InpcAnimated.ParameterMap[npcAnimationParameterType], isOn);
        }

        else
        {
            Debug.LogError("The playerAnimationParameterType is missing from the ParameterMap dictionary. Please add it.");
        }
    }
}
