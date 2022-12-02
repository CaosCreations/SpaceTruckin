using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DateTimeText : MonoBehaviour
{
    // Todo: Change to TMP 
    [SerializeField]
    private Text dateTimeText;

    void Update()
    {
        if (dateTimeText == null)
        {
            Debug.LogError(nameof(dateTimeText) + " was null. Cannot update text.");
            return;
        }
        dateTimeText.text = ClockManager.GetDateTimeText();
    }
}
