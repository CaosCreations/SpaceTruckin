using UnityEditor;
using UnityEngine;

public class SetDateTimeWindow : EditorWindow
{
    // Use strings for input fields to validate leading zeros
    private string day, month, year;
    private string hours, minutes, seconds;
    private static Vector2 maxWindowSize = new(220, 460);
    private static Vector2 minWindowSize = maxWindowSize;
    private bool CanEditTime => Application.isPlaying;

    [MenuItem("Space Truckin/Calendar/Set DateTime")]
    private static void Init()
    {
        SetDateTimeWindow window = GetWindow<SetDateTimeWindow>();
        window.titleContent = new GUIContent("Set DateTime");
        window.maxSize = maxWindowSize;
        window.minSize = minWindowSize;
        window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        PopulateForm();
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
    {
        if (stateChange == PlayModeStateChange.EnteredPlayMode)
        {
            PopulateForm();
        }
    }

    private void PopulateForm()
    {
        CalendarData calendarData = EditorHelper.GetAsset<CalendarData>();
        Date currentDate = calendarData.CurrentDate;
        day = currentDate.Day.ToString();
        month = currentDate.Month.ToString();
        year = currentDate.Year.ToString();

        if (CanEditTime)
        {
            TimeOfDay timeOfDay = ClockManager.CurrentTime.ToTimeOfDay();
            hours = timeOfDay.Hours.ToString();
            minutes = timeOfDay.Minutes.ToString();
            seconds = timeOfDay.Seconds.ToString();
        }
    }

    private void OnGUI()
    {
        float padding = 10f;

        EditorGUI.DropShadowLabel(
            new Rect(padding, padding, position.width - 2 * padding, 20), "Enter Date values: ");

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

        if (CanEditTime)
        {
            EditorGUI.DropShadowLabel(new Rect(padding, 200, fieldWidth, 20), "Enter Time values: ");

            // Hours field
            EditorGUI.LabelField(new Rect(padding, 230, fieldWidth, 20), "Hours");
            hours = EditorGUI.TextField(new Rect(padding, 250, fieldWidth, 20), hours);
            hours = ValidateLeadingZero(hours);

            // Minutes field
            EditorGUI.LabelField(new Rect(padding, 280, fieldWidth, 20), "Minutes");
            minutes = EditorGUI.TextField(new Rect(padding, 300, fieldWidth, 20), minutes);
            minutes = ValidateLeadingZero(minutes);

            // Seconds field
            EditorGUI.LabelField(new Rect(padding, 330, fieldWidth, 20), "Seconds");
            seconds = EditorGUI.TextField(new Rect(padding, 350, fieldWidth, 20), seconds);
            seconds = ValidateLeadingZero(seconds);
        }

        float buttonY = CanEditTime ? position.height - 30 - padding : 200;

        if (GUI.Button(new Rect(padding, buttonY, fieldWidth, 20), "Set Date/Time"))
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

            if (CanEditTime)
            {
                int parsedHours = int.Parse(hours);
                int parsedMinutes = int.Parse(minutes);
                int parsedSeconds = int.Parse(seconds);
                int totalSeconds = (parsedHours * 3600) + (parsedMinutes * 60) + parsedSeconds;

                ClockManager.SetCurrentTime(totalSeconds);
            }
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
