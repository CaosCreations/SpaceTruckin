using PixelCrushers.DialogueSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IntroAnimationDialogueSystemTrigger), true)]
public class IntroAnimationDialogueSystemTriggerEditor : DialogueSystemTriggerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Intro Animation", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationRegistry"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animationPlayer"));

        serializedObject.ApplyModifiedProperties();
    }
}
