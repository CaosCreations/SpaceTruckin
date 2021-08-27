using System;
using UnityEngine;

public class AnimationManager<T> : MonoBehaviour where T : Enum
{
    protected void LogMissingParameterMapping(T parameterType)
    {
        string parameterTypeName = Enum.GetName(typeof(PlayerAnimationParameterType), parameterType);

        Debug.LogError($"The PlayerAnimationParameterType '{parameterTypeName}' is missing from the ParameterMap Dictionary.");
    }
}
