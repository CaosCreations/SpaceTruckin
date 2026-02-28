using UnityEditor;
using UnityEngine;

namespace PixelCrushers
{

    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return IsConditionalBoolSetToRequiredValue(property)
                ? base.GetPropertyHeight(property, label)
                : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsConditionalBoolSetToRequiredValue(property)) return;
            EditorGUI.PropertyField(position, property, label, true);
        }

        private bool IsConditionalBoolSetToRequiredValue(SerializedProperty property)
        {
            var showIfAttribute = attribute as ShowIfAttribute;
            var conditionalProperty = property.serializedObject.FindProperty(showIfAttribute.conditionalBoolName);
            if (conditionalProperty != null)
            {
                return conditionalProperty.boolValue == showIfAttribute.conditionalBoolValue;
            }
            else
            {
                return false;
            }
        }

    }

}
