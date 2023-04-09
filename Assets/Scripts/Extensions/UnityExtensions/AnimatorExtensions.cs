using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class AnimatorExtensions
{
    public static void ResetBools(this Animator self)
    {
        self.SetAllBools(false);
    }

    public static void SetAllBools(this Animator self, bool value)
    {
        foreach (var parameter in self.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                self.SetBool(parameter.name, value);
            }
        }
    }

    public static bool ContainsParameterWithName(this Animator self, string parameterName)
    {
        return self.parameters.FirstOrDefault(p => p.name == parameterName) != null;
    }

    public static string GetFirstTrueBool(this Animator self)
    {
        var param = self.parameters.FirstOrDefault(p => p.type == AnimatorControllerParameterType.Bool && self.GetBool(p.name));
        return param != null ? param.name : default;
    }

    public static bool IsPlaying(this Animator self)
    {
        return self.GetCurrentAnimatorStateInfo(0).length > self.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static bool IsPlaying(this Animator self, string stateName)
    {
        return self.IsPlaying() && self.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public static AnimationClip GetClipByName(this Animator self, string clipName)
    {
        return self.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == clipName);
    }

    public static void PlayOnce(this Animator self, string playClipName, string resetClipName)
    {
        var playClip = self.GetClipByName(playClipName);

        if (playClip == null)
            throw new ArgumentException("Animation clip doesn't exist with name: " + playClipName);

        // Play the clip to play once
        //self.CrossFadeInFixedTime(playClipName, 0.2f);
        self.Play(playClipName);

        var behaviour = self.GetComponent<MonoBehaviour>();

        // Wait for the clip to finish playing
        behaviour.StartCoroutine(WaitForClipToFinish(playClip.length));

        IEnumerator WaitForClipToFinish(float clipLength)
        {
            // Wait until the clip has finished playing
            var elapsedTime = 0f;
            while (elapsedTime < clipLength)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Return to the original clip
            //self.CrossFadeInFixedTime(resetClipName, 0.2f);
            self.Play(resetClipName);
        }
    }
}
