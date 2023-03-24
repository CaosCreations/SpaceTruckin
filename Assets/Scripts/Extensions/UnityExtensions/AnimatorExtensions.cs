﻿using System.Linq;
using UnityEngine;

public static class AnimatorExtensions
{
    public static void ResetAndSetBoolTrue(this Animator self, string parameterName)
    {
        self.SetAllBoolParameters(false);
        self.SetBool(parameterName, true);
    }

    public static void SetAllBoolParameters(this Animator self, bool value)
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
}
