using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionScheduleSlot : MonoBehaviour, IPointerClickHandler
{
    public RectTransform parentTransform;
    public RectTransform missionLayoutContainer;
    public Image slotImage;
    private MissionsUI missionsUI;

    public int hangarNode;
    public Pilot Pilot { get; set; }

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

    private void Start()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (missionsUI != null && Pilot == null)
            {
                // Allow the player to dock a ship at a node without dragging on a mission
                missionsUI.PopulatePilotSelect(this);
            }
        }
    }
}
