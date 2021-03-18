using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotSelectItem : MonoBehaviour, IPointerClickHandler
{
    private Pilot pilot;
    private Mission mission;
    private MissionsUI missionsUI;
    private MissionScheduleSlot scheduleSlot;
    [SerializeField] private Button itemButton; 
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemImage;

    private void Start()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
    }

    public void Init(Pilot pilot, MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        this.pilot = pilot;
        this.scheduleSlot = scheduleSlot;
        this.mission = mission;
        itemButton.AddOnClick(SelectPilotForSlot);
        itemText.SetText(BuildItemString());
        itemImage.sprite = pilot.Avatar;
    }

    public void SelectPilotForSlot()
    {
        PilotSelectItem itemToReplace = scheduleSlot.missionLayoutContainer
            .GetComponentInChildren<PilotSelectItem>();
        
        if (itemToReplace != null)
        {
            // If there is already a pilot in the slot, remove it
            MissionsManager.RemoveScheduledMission(itemToReplace.pilot);
            Destroy(itemToReplace.gameObject);
        }
        transform.SetParent(scheduleSlot.missionLayoutContainer);
        HangarManager.DockShip(pilot.Ship, scheduleSlot.hangarNode);

        if (mission != null)
        {
            MissionsManager.AddOrUpdateScheduledMission(pilot, mission);
        }

        missionsUI.scrollViewContent.transform.DestroyDirectChildren();

        // Return to the mission select after a pilot has been selected
        missionsUI.PopulateMissionSelect();
    }

    public void DeselectPilot() 
    {
        MissionsManager.RemoveScheduledMission(pilot);
        Destroy(gameObject);
    }

    public void ReselectPilot()
    {
        MissionsManager.RemoveScheduledMission(pilot);
        missionsUI.PopulatePilotSelect(scheduleSlot);
    }

    private string BuildItemString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks(pilot.Name);
        builder.AppendLineWithBreaks(pilot.Ship.Name);
        builder.AppendLineWithBreaks($"Lv. {pilot.Level}");
        return builder.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    if (PilotsManager.PilotHasMission(pilot) && transform.parent.GetComponentInParent<MissionScheduleSlot>() != null)
        //    {
        //        // Allow the player to reselect the pilot once it's in the slot
        //        ReselectPilot();
        //    }
        //}
    }
}
