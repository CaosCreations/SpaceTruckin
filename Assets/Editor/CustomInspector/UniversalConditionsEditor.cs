using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object), true)]
public class UniversalConditionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        SerializedProperty conditionsProperty = serializedObject.FindProperty("conditions");
        if (conditionsProperty == null || !conditionsProperty.isArray || conditionsProperty.GetType() != typeof(Condition))
        {
            return;
        }

        EditorGUILayout.LabelField("Conditions");

        for (int i = 0; i < conditionsProperty.arraySize; i++)
        {
            SerializedProperty conditionProperty = conditionsProperty.GetArrayElementAtIndex(i);
            SerializedProperty typeProperty = conditionProperty.FindPropertyRelative("Type");

            EditorGUILayout.PropertyField(typeProperty, new GUIContent("Condition Type"));

            ConditionType conditionType = (ConditionType)typeProperty.enumValueIndex;

            EditorGUI.indentLevel++;

            switch (conditionType)
            {
                case ConditionType.Date:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("ActiveDates"), true);
                    break;
                case ConditionType.Message:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Message"), true);
                    break;
                case ConditionType.Mission:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Mission"), true);
                    break;
                case ConditionType.DialogueVariable:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("DialogueVariableName"), true);
                    break;
            }

            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Add Condition"))
        {
            conditionsProperty.InsertArrayElementAtIndex(conditionsProperty.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("Preview");
    }
}
