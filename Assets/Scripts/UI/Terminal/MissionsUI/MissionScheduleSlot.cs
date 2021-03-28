using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionScheduleSlot : MonoBehaviour, IPointerClickHandler
{
    public RectTransform parentTransform;
    public RectTransform layoutContainer;
    public Image slotImage;
    public GameObject pilotInSlotPrefab;
    private MissionsUI missionsUI;

    public int hangarNode;
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

    public void PutPilotInSlot(Pilot pilot)
    {
        CleanPilotInSlot();
        PilotInMissionScheduleSlot pilotInSlot = Instantiate(pilotInSlotPrefab, layoutContainer.transform)
            .GetComponent<PilotInMissionScheduleSlot>();

        pilotInSlot.Init(pilot);
    }

    public void CleanSlot()
    {
        layoutContainer.transform.DestroyDirectChildren();
    }

    public void CleanPilotInSlot()
    {
        PilotInMissionScheduleSlot pilotInSlot = layoutContainer.GetComponentInChildren<PilotInMissionScheduleSlot>();
        if (pilotInSlot != null)
        {
            Destroy(pilotInSlot.gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (missionsUI != null 
                && IsActive 
                && layoutContainer.childCount <= 0)
            {
                // Allow the player to dock a ship at a node without scheduling a mission
                missionsUI.PopulatePilotSelect(this);
            }
        }
    }
}
