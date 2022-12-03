using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DateTimeText : MonoBehaviour
{
    // Todo: Change to TMP 
    [SerializeField]
    private Text dateTimeText;

    private void Update()
    {
        if (dateTimeText == null
            || dateTimeText.gameObject == null
            || !dateTimeText.gameObject.activeSelf)
        {
            return;
        }

        dateTimeText.text = ClockManager.GetDateTimeText();
    }

    public void SetActive(bool active)
    {
        if (dateTimeText == null)
        {
            Debug.LogWarning("Can't set datetime text object to active/inactive because it is null.");
            return;
        }
        dateTimeText.SetActive(active);
    }
}
