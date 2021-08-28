using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimationUtils
{
    public static List<string> GetIdleParameterNames(Animator animator)
    {
        var idleAnimationKeys = animator.parameters
            .Select(x => x.name)
            .Where(x => x.StartsWith(
                AnimationConstants.NpcIdleParameterPrefix, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        return idleAnimationKeys;
    }
}
