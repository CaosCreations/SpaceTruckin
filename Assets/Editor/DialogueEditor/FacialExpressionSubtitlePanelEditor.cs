using PixelCrushers.DialogueSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FacialExpressionSubtitlePanel), true)]
public class FacialExpressionSubtitlePanelEditor : StandardUISubtitlePanelEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Composite Portrait", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("compositePortrait"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("faceDataRegistry"));

        serializedObject.ApplyModifiedProperties();
    }
}
