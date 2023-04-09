using System;
using System.Linq;
using UnityEngine;

public class InputAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private AnimationByInput[] animationsByInputs;

    private void Awake()
    {
        foreach (var inputMap in animationsByInputs)
        {
            AddOnAnimationFinishedEvent(inputMap.Animator, inputMap.PlayClipName, inputMap.ResetClipName);
        }
    }

    /// <summary>
    /// Adds an animation event that will fire when the play clip finishes.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    private void AddOnAnimationFinishedEvent(UnityEngine.Object animatorObj, string playClipName, string resetClipName)
    {
        Animator animator = animatorObj as Animator;
        AnimationClip clip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == playClipName);

        if (clip == null)
            throw new ArgumentException("Clip to play doesn't exist on animator: " + playClipName);

        AnimationEvent animEvent = new()
        {
            time = clip.length,
            functionName = "OnAnimationFinished",
            stringParameter = resetClipName,
            objectReferenceParameter = animatorObj
        };

        clip.AddEvent(animEvent);
    }

    private void Update()
    {
        foreach (var inputMap in animationsByInputs)
        {
            if (Input.GetKeyDown(inputMap.KeyCode)
                || (!string.IsNullOrWhiteSpace(inputMap.ButtonName) && Input.GetButtonDown(inputMap.ButtonName)))
            {
                inputMap.Animator.Play(inputMap.PlayClipName);
            }
        }
    }

    /// <summary>
    /// The animation event handler.
    /// </summary>
    private void OnAnimationFinished(UnityEngine.Object animatorObj, string resetClipName)
    {
        Animator animator = animatorObj as Animator;

        // Stop the current clip
        animator.enabled = false;

        // Play a different clip
        animator.enabled = true;
        animator.Play(resetClipName);
    }
}
