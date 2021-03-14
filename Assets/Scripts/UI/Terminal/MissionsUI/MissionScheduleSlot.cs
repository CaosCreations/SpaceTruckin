using UnityEngine;
using UnityEngine.UI;

public class MissionScheduleSlot : MonoBehaviour
{
    public RectTransform parentTransform;
    public RectTransform slotTransform;
    public Image slotImage;

    public int hangarNode;
    public Pilot Pilot { get; set; } // Maybe more getter logic here

    private bool isActive;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (value)
            {
                slotImage.color = Color.white;
            }
            else
            {
                slotImage.color = Color.grey;
            }
        }
    }
}
