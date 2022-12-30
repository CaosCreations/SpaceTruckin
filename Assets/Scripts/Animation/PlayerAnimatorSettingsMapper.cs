using System;
using UnityEngine;

public class PlayerAnimatorSettingsMapper : IAnimatorSettingsMapper
{
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
