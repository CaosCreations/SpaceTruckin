using UnityEngine;
using UnityEngine.UI;

public class MissionScheduleSlot : MonoBehaviour
{
    public RectTransform parentTransform;
    public RectTransform slotTransform;
    public Image slotImage;

    public HangarNode hangarNode;
    public Ship ship;

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

    public int ShipId { get => ship.saveData.id; }
}
