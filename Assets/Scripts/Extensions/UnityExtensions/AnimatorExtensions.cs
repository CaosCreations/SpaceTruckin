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
}
