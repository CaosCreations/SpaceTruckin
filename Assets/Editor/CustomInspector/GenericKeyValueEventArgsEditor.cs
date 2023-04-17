using Events;
using System;
using UnityEditor;

[CustomEditor(typeof(SerializedGenericKeyValueEventArgs))]
public class GenericKeyValueEventArgsEditor : Editor
{
    private readonly string[] _typeNames = { "string", "int", "float", "bool" };
    private int _selectedTypeIndex = 0;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty eventDataProperty = serializedObject.FindProperty("EventData");
        SerializedProperty variableNameProperty = eventDataProperty.FindPropertyRelative("Key");
        SerializedProperty valueProperty = eventDataProperty.FindPropertyRelative("Value");

        EditorGUILayout.PropertyField(variableNameProperty);

        if (variableNameProperty.objectReferenceValue != null)
        {
            System.Type type = variableNameProperty.objectReferenceValue.GetType();
            object value = Convert.ChangeType(valueProperty.objectReferenceValue, type);
            valueProperty.objectReferenceValue = EditorGUILayout.ObjectField(valueProperty.displayName, valueProperty.objectReferenceValue, type, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}