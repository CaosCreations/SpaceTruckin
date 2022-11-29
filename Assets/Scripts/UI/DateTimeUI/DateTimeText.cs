using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DateTimeText : MonoBehaviour
{
    // Todo: Change to TMP 
    [SerializeField]
    private Text dateTimeText;

    // Start is called before the first frame update
    void Start()
    {
        //dateTimeText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
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
