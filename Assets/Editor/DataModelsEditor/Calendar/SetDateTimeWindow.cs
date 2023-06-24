using UnityEditor;
using UnityEngine;

public class SetDateTimeWindow : EditorWindow
{
    // Use strings for input fields to validate leading zeros
    private string day, month, year;
    // TODO: Support time of day
    private int hours, minutes, seconds;

    [MenuItem("Space Truckin/Calendar/Set DateTime")]
    private static void Init()
    {
        SetDateTimeWindow window = GetWindow<SetDateTimeWindow>();
        window.titleContent = new GUIContent("Set DateTime");
        window.maxSize = new Vector2(220, 240);
        window.Show();
    }

    private void OnGUI()
    {
        float padding = 10f;

        EditorGUI.DropShadowLabel(
            new Rect(padding, padding, position.width - 2 * padding, 20), "Enter Date components: ");

        float fieldWidth = position.width - 2 * padding;

        // Day field
        EditorGUI.LabelField(new Rect(padding, 40, fieldWidth, 20), "Day");
        day = EditorGUI.TextField(new Rect(padding, 60, fieldWidth, 20), day);
        day = ValidateLeadingZero(day);

        // Month field
        EditorGUI.LabelField(new Rect(padding, 90, fieldWidth, 20), "Month");
        month = EditorGUI.TextField(new Rect(padding, 110, fieldWidth, 20), month);
        month = ValidateLeadingZero(month);

        // Year field
        EditorGUI.LabelField(new Rect(padding, 140, fieldWidth, 20), "Year");
        year = EditorGUI.TextField(new Rect(padding, 160, fieldWidth, 20), year);
        year = ValidateLeadingZero(year);

        if (GUI.Button(new Rect(padding, 200, fieldWidth, 20), "Set Date"))
        {
            int parsedDay = int.Parse(day);
            int parsedMonth = int.Parse(month);
            int parsedYear = int.Parse(year);

            CalendarEditor.SetDate(new Date()
            {
                Day = parsedDay,
                Month = parsedMonth,
                Year = parsedYear
            }.Validate());
        }

        Repaint();
    }

    private string ValidateLeadingZero(string input)
    {
        if (string.IsNullOrEmpty(input))

            return input;

        if (input.Length > 1 && input.StartsWith("0"))
            return input.TrimStart('0');

        return input;
    }
}
