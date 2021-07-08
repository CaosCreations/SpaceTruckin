using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private AnimationData[] animationData;

    private Dictionary<string, AnimationData> animationDataDictionary;

    private void Awake()
    {
        animationDataDictionary = new Dictionary<string, AnimationData>();

        foreach (AnimationData item in animationData)
        {
            animationDataDictionary.Add(item.AnimationDataName, item);
        }
    }

    public void PlayAnimation(string animationDataName, bool isOn)
    {
        Debug.Log(animationDataDictionary[animationDataName]);
        animator.SetBool(animationDataDictionary[animationDataName].AnimationParameter, isOn);
    }
}


