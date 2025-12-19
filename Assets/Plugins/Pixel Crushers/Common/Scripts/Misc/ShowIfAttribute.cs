using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Decorator attribute to show propery "b" if bool property "a" is true.
    /// Syntax: [ShowIf("a")] bool b;
    /// </summary>
    public class ShowIfAttribute : PropertyAttribute
    {

        public string conditionalBoolName { get; private set; }

        public ShowIfAttribute(string conditionalBool)
        {
            this.conditionalBoolName = conditionalBool;
        }

    }

}
