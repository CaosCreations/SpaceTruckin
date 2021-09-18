using System.Linq;
using UnityEngine;

public static class AnimatorExtensions
{
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
        return self.parameters.FirstOrDefault(x => x.name == parameterName) != null;
    }
}
