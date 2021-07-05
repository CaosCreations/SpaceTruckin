using UnityEngine;

public class CalendarEditor : MonoBehaviour
{
    public static void DeleteSaveData()
    {
        var calendarData = EditorHelper.GetAsset<CalendarData>();
        SaveDataEditor.DefaultStructFields(ref calendarData.CurrentDate);
    }
}
