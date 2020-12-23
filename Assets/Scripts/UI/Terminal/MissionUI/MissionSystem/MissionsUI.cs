using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject missionSchedule;

    public MissionScheduleSlot[] missionSlots;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        SetupMissionSlots();
        CleanMenu();
        PopulateScrollView();
    }

    void SetupMissionSlots()
    {
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            Ship ship = ShipsManager.NodeHasShip(slot.hangarNode);
            if (ship != null)
            {
                slot.ship = ship;
                if(ship.currentMission == null)
                {
                    slot.IsActive = true;
                }
                else
                {
                    slot.IsActive = !ship.currentMission.IsInProgress();
                }
            }
            else
            {
                slot.IsActive = false;
            }
        }
    }

    void CleanMenu()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(MissionScheduleSlot slot in missionSlots)
        {
            if(slot.slotTransform.childCount > 0)
            {
                Destroy(slot.slotTransform.GetChild(0).gameObject);
            }
        }
    }

    void PopulateScrollView()
    {
        foreach (Mission mission in MissionsManager.GetAcceptedMissions())
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission, scrollViewContent.transform);
        }

        List<Mission> scheduledMissions = MissionsManager.GetScheduledMissions();
        foreach(MissionScheduleSlot slot in missionSlots)
        {
            Mission missionForSlot = scheduledMissions.Where(x => x.ship == slot.ship).FirstOrDefault();

            if(missionForSlot != null)
            {
                GameObject scrollItem = Instantiate(missionItemPrefab, slot.slotTransform);
                MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                missionItem.Init(missionForSlot, scrollViewContent.transform);
            }
        }
    }

    public MissionScheduleSlot GetSlotForMissionDrag(Vector2 position)
    {
        foreach(MissionScheduleSlot slot in missionSlots)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(slot.parentTransform, position)
                && slot.IsActive)
            {
                return slot;
            }
        }

        return null;
    }

    public void DisplayMissionDetails(MissionUIItem item)
    {
        GameObject missionDetails = new GameObject("Mission Details");
        missionDetails.transform.parent = missionSchedule.transform;
        RectTransform rect = missionDetails.GetComponent<RectTransform>();

        Vector2 anchorMin = new Vector2();
        Vector2 anchorMax = new Vector2();
        
        if (item.transform.localPosition.y < 0.33f)
        {
            rect.anchorMin = new Vector2(0f, 0.66f);
            rect.anchorMax = new Vector2(1f, 1f);
        }
        else if (item.transform.localPosition.y >= 0.33f && item.transform.localPosition.y < 0.66f)
        {
            rect.anchorMin = new Vector2(0f, 0.33f);
            rect.anchorMax = new Vector2(1f, 0.66f);
        }
        else
        {
            rect.anchorMin = new Vector2(0f, 0.33f);
            rect.anchorMax = new Vector2(1f, 0f);
        }

        Text detailsText = missionDetails.AddComponent<Text>();
        detailsText.text = BuildDetailsText(item.mission);

    }

    private string BuildDetailsText( Mission mission)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Name: " + mission.missionName);
        builder.AppendLine("Customer: " + mission.customer);
        builder.Append("Cargo: " + mission.cargo);

        if (string.IsNullOrEmpty(mission.description))
        {
            mission.description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";
        }

        builder.AppendLine("Description: " + mission.description);
        builder.AppendLine("Reward: " + mission.reward);
        return builder.ToString();
    }
}
