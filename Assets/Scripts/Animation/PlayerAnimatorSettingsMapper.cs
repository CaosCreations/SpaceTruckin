using System;
using UnityEngine;

public class PlayerAnimatorSettingsMapper : IAnimatorSettingsMapper
{
    public Animator MapSettings(Animator source, Animator target)
    {
        target.runtimeAnimatorController = source.runtimeAnimatorController;
        target.rootPosition = source.rootPosition;
        target.rootRotation = source.rootRotation;
        target.applyRootMotion = source.applyRootMotion;
        return target;
    }

    public Animator MapSettings(Animator target, AnimatorSettings settings)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        target.avatar = settings.Avatar;
        target.runtimeAnimatorController = settings.AnimatorController;
        return target;
    }
}
