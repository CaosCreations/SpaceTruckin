using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TemporaryConditionalObject))]
public class TemporaryConditionalObjectEditor : Editor
{
    private SerializedProperty conditionsProperty;

    private void OnEnable()
    {
        conditionsProperty = serializedObject.FindProperty("conditions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Conditions");
        for (int i = 0; i < conditionsProperty.arraySize; i++)
        {
            SerializedProperty conditionProperty = conditionsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Type"));

            TemporaryConditionalObject.ConditionType conditionType = (TemporaryConditionalObject.ConditionType)conditionProperty.FindPropertyRelative("Type").enumValueIndex;

            switch (conditionType)
            {
                case TemporaryConditionalObject.ConditionType.Date:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("ActiveDates"), true);
                    break;
                case TemporaryConditionalObject.ConditionType.Message:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Message"), true);
                    break;
                case TemporaryConditionalObject.ConditionType.Mission:
                    EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Mission"), true);
                    break;
            }
        }

        if (GUILayout.Button("Add Condition"))
        {
            conditionsProperty.InsertArrayElementAtIndex(conditionsProperty.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
