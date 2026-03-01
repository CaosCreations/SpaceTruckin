using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Decorator attribute to show propery "b" if bool property "a" is true,
    /// or if "a" is false if you specify false as the second parameter.
    /// 
    /// Syntax: [ShowIf("a")] bool b;
    /// Explanation: Shows b if a is true.
    /// 
    /// Syntax: [ShowIf("a", false)] bool b;
    /// Explanation: Shows b if a is false.
    /// </summary>
    public class ShowIfAttribute : PropertyAttribute
    {

        public string conditionalBoolName { get; private set; }

        public bool conditionalBoolValue { get; private set; }

        public ShowIfAttribute(string conditionalBool, bool conditionalBoolValue = true)
        {
            this.conditionalBoolName = conditionalBool;
            this.conditionalBoolValue = conditionalBoolValue;
        }

    }

}
