using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorData : MonoBehaviour
{
    public Animator Animator;

    [SerializeField] private AnimatorController animatorController;

    [SerializeField] private AnimationParameterKeyValuePair[] animationParameterKeyValuePair;

    // Index the Dictionary with the enum value, so no human error. 
    public Dictionary<AnimationParemeterType, string> ParameterMap;

    private void Awake()
    {
        if (Animator == null)
            Debug.LogError("The animator is null. In the editor, please drag in the animator of the target game object.");

        if (animatorController == null)
            Debug.LogError("The animatorController is null. In the editor, please drag in the animatorController of the target game object.");

        ParameterMap = new Dictionary<AnimationParemeterType, string>();

        AnimatorControllerParameter[] animatorControllerParameter = animatorController.parameters;


        for (int i = 0; i < animationParameterKeyValuePair.Length; i++)
        {
            if(animationParameterKeyValuePair[i].AnimationParameterValue == string.Empty)
            {
                Debug.LogError("The AnimationParameterValue is empty. " +
                               "Please input a valid string that is identical to a parameter of the target Animator.");
            }

            bool parameterAndStringMatch = false;

            foreach(AnimatorControllerParameter parameter in animatorControllerParameter)
            {
                if (animationParameterKeyValuePair[i].AnimationParameterValue == parameter.name)
                    parameterAndStringMatch = true;
            }

            if (parameterAndStringMatch == false)
                Debug.LogError("The AnimationParameterValue from the animationParameterKeyValuePair :"
                           + animationParameterKeyValuePair[i].AnimationParameterValue + " doesn't match any of the paramater names of the animator controller. " +
                           "Please fix this animationParameterValue so that it does."
                           );

            ParameterMap.Add(animationParameterKeyValuePair[i].AnimationParameterKey, 
                             animationParameterKeyValuePair[i].AnimationParameterValue);
        }
    }
}


