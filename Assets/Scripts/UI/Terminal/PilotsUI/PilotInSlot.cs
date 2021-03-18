using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotInSlot : MonoBehaviour, IPointerClickHandler
{
    public Pilot pilot;

    public void Init(Pilot pilot)
    {
        this.pilot = pilot;
        Image image = GetComponent<Image>();
        if (pilot != null && image != null)
        {
            image.sprite = pilot.Avatar;
        }
    }

    private void RemovePilot()
    {
        if (pilot != null)
        {
            MissionsManager.RemoveScheduledMission(pilot);
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RemovePilot();

            // Remove the mission as it's redundant without a pilot
            MissionUIItem missionInSlot = transform.parent.GetComponentInChildren<MissionUIItem>();
            if (missionInSlot != null)
            {
                missionInSlot.Unschedule();
            }
        }
    }
}
