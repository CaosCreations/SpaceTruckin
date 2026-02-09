using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FacialExpressionTester))]
public class FacialExpressionTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tester = (FacialExpressionTester)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Quick Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh"))
        {
            tester.RefreshPortrait();
        }
        if (GUILayout.Button("Cycle Expression"))
        {
            tester.CycleExpression();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < 9; i++)
        {
            if (GUILayout.Button(i.ToString(), GUILayout.Width(30)))
            {
                tester.SetExpression(i);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "Index 0 = Default portrait\n" +
            "Index 1-8 = Alternate portraits",
            MessageType.Info);
    }
}
