using UnityEditor;
using UnityEngine;

public class SetDateTimeWindow : EditorWindow
{
    // Use ints for EditorGUI input fields 
    private int day, month, year;
    private int hours, minutes, seconds;

    [MenuItem("Space Truckin/Calendar/Set DateTime")]
    private static void Init()
    {
        EditorWindow window = GetWindow(typeof(SetDateTimeWindow));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUI.DropShadowLabel(
            new Rect(0, 0, position.width - 40, 20), "Enter Date components: ");

        // Input fields
        day = EditorGUI.IntField(new Rect(20, 20, position.width - 40, 20), day);
        month = EditorGUI.IntField(new Rect(20, 60, position.width - 40, 20), day);
        year = EditorGUI.IntField(new Rect(20, 100, position.width - 40, 20), day);

        //if (GUI.Button(new Rect(20, 40, position.width - 40, 20), "Set Date"))
        //{

        //}

        Repaint();
    }
}
