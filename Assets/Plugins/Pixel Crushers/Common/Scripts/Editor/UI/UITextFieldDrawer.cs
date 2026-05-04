// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers
{

    [CustomPropertyDrawer(typeof(UITextField), true)]
    public class UITextFieldDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            try
            {
                var uiTextProperty = property.FindPropertyRelative("m_uiText");
                var textMeshProUGUIProperty = property.FindPropertyRelative("m_textMeshProUGUI");
                var superTextMeshProperty = property.FindPropertyRelative("m_superTextMesh");
                var uniTextProperty = property.FindPropertyRelative("m_uniText");
                var isUiTextAssigned = (uiTextProperty != null) && (uiTextProperty.objectReferenceValue != null);
                var isTextMeshProUGUIAssigned = (textMeshProUGUIProperty != null) && (textMeshProUGUIProperty.objectReferenceValue != null);
                var isSuperTextMeshAssigned = (superTextMeshProperty != null) && (superTextMeshProperty.objectReferenceValue != null);
                var isUniTextAssigned = (uniTextProperty != null) && (uniTextProperty.objectReferenceValue != null);
                var isContentAssigned = isUiTextAssigned || isTextMeshProUGUIAssigned || isSuperTextMeshAssigned || isUniTextAssigned;
                int numUnassignedLines = 1;
                if (textMeshProUGUIProperty != null) numUnassignedLines++;
                if (superTextMeshProperty != null) numUnassignedLines++;
                if (uniTextProperty != null) numUnassignedLines++;
                return (isContentAssigned ? 1 : numUnassignedLines) * EditorGUIUtility.singleLineHeight;
            }
            catch (System.ArgumentException) // Handles IMGUI->UITK bug in Unity 2022.2.
            {
                return 2 * EditorGUIUtility.singleLineHeight;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                EditorGUI.BeginProperty(position, label, property);

                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

                var uiTextProperty = property.FindPropertyRelative("m_uiText");
                var textMeshProUGUIProperty = property.FindPropertyRelative("m_textMeshProUGUI");
                var superTextMeshProperty = property.FindPropertyRelative("m_superTextMesh");
                var uniTextProperty = property.FindPropertyRelative("m_uniText");
                if (uiTextProperty == null)
                {
                    Debug.LogError("Sorry! There was an internal editor error with a UI Text Field. Please contact Pixel Crushers for support.");
                    return;
                }
                var isUiTextAssigned = (uiTextProperty != null) && (uiTextProperty.objectReferenceValue != null);
                var isTextMeshProUGUIAssigned = (textMeshProUGUIProperty != null) && (textMeshProUGUIProperty.objectReferenceValue != null);
                var isSuperTextMeshAssigned = (superTextMeshProperty != null) && (superTextMeshProperty.objectReferenceValue != null);
                var isUniTextAssigned = (uniTextProperty != null) && (uniTextProperty.objectReferenceValue != null);
                var isContentAssigned = isUiTextAssigned || isTextMeshProUGUIAssigned || isSuperTextMeshAssigned || isUniTextAssigned;

                float yOffset = 0;

                if (isUiTextAssigned || !isContentAssigned)
                {
                    EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), uiTextProperty, GUIContent.none);
                    yOffset += EditorGUIUtility.singleLineHeight;
                }

                if (isTextMeshProUGUIAssigned || (textMeshProUGUIProperty != null && !isContentAssigned))
                {
                    EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), textMeshProUGUIProperty, GUIContent.none);
                    yOffset += EditorGUIUtility.singleLineHeight;
                }

                if (isSuperTextMeshAssigned || (superTextMeshProperty != null && !isContentAssigned))
                {
                    EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), superTextMeshProperty, GUIContent.none);
                    yOffset += EditorGUIUtility.singleLineHeight;
                }

                if (isUniTextAssigned || (uniTextProperty != null && !isContentAssigned))
                {
                    EditorGUI.PropertyField(new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight), uniTextProperty, GUIContent.none);
                    yOffset += EditorGUIUtility.singleLineHeight;
                }
            }
            finally
            {
                EditorGUI.EndProperty();
            }
        }

    }
}
