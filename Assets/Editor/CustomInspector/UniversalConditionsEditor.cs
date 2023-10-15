//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(Object), true)]
//public class UniversalConditionsEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        SerializedProperty conditionsProperty = serializedObject.FindProperty("conditions");

//        bool shouldDrawDefaultInspector = conditionsProperty != null && conditionsProperty.isArray;

//        if (shouldDrawDefaultInspector)
//        {
//            EditorGUI.BeginChangeCheck();
//            DrawDefaultInspector();
//            if (EditorGUI.EndChangeCheck())
//            {
//                return;
//            }
//        }

//        if (shouldDrawDefaultInspector)
//        {
//            EditorGUILayout.LabelField("Conditions");

//            for (int i = 0; i < conditionsProperty.arraySize; i++)
//            {
//                SerializedProperty conditionProperty = conditionsProperty.GetArrayElementAtIndex(i);
//                SerializedProperty typeProperty = conditionProperty.FindPropertyRelative("Type");

//                EditorGUILayout.PropertyField(typeProperty, new GUIContent("Condition Type"));

//                ConditionType conditionType = (ConditionType)typeProperty.enumValueIndex;

//                EditorGUI.indentLevel++;

//                switch (conditionType)
//                {
//                    case ConditionType.Date:
//                        EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("ActiveDates"), true);
//                        break;
//                    case ConditionType.Message:
//                        EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Message"), true);
//                        break;
//                    case ConditionType.Mission:
//                        EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("Mission"), true);
//                        break;
//                    case ConditionType.DialogueVariable:
//                        EditorGUILayout.PropertyField(conditionProperty.FindPropertyRelative("DialogueVariableName"), true);
//                        break;
//                }

//                EditorGUI.indentLevel--;
//            }

//            if (GUILayout.Button("Add Condition"))
//            {
//                conditionsProperty.InsertArrayElementAtIndex(conditionsProperty.arraySize);
//            }
//        }

//        serializedObject.ApplyModifiedProperties();
//    }

//    public override bool RequiresConstantRepaint()
//    {
//        return true;
//    }

//    public override bool HasPreviewGUI()
//    {
//        return true;
//    }

//    public override GUIContent GetPreviewTitle()
//    {
//        return new GUIContent("Preview");
//    }
//}
