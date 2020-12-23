using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject missionDetailsPrefab;
    public GameObject missionSchedule;
    public GameObject missionDetails; 

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

    public void CreateMissionDetails(MissionUIItem listItem)
    {
        DestroyMissionDetails();
        missionDetails = new GameObject("MissionDetails");
        missionDetails.transform.parent = transform;
        RectTransform listRect = listItem.GetComponent<RectTransform>();
        RectTransform detailsRect = missionDetails.AddComponent<RectTransform>();
        detailsRect.ResetRect();
        missionDetails.AddComponent<Image>().color = Color.magenta;
        
        detailsRect.SetAnchors(GetMissionDetailsAnchors(listRect));

        GameObject textContainer = new GameObject("MissionDetailsText");
        textContainer.transform.parent = missionDetails.transform;
        Text detailsText = textContainer.AddComponent<Text>();
        RectTransform textRect = textContainer.GetComponent<RectTransform>();
        textRect.ResetRect();
        textRect.StretchAnchors();
        detailsText.text = BuildDetailsText(listItem.mission);
        detailsText.SetDefaultFont();
        detailsText.resizeTextForBestFit = true;
        detailsText.color = Color.black;
    }

    private ValueTuple<Vector2, Vector2> GetMissionDetailsAnchors(RectTransform listRect)
    {
        var anchors = new ValueTuple<Vector2, Vector2>();
        anchors.Item1.x = 0.55f;
        anchors.Item2.x = 0.95f;

        // Place the details in line with the corresponding mission
        if (listRect.localPosition.y < 0.33f)
        {
            anchors.Item1.y = 0.65f;
            anchors.Item2.y = 0.95f;
        }
        else if (listRect.localPosition.y >= 0.33f && listRect.localPosition.y < 0.66f)
        {
            anchors.Item1.y = 0.35f;
            anchors.Item2.y = 0.65f;
        }
        else
        {
            anchors.Item1.y = 0.05f;
            anchors.Item2.y = 0.35f;
        }
        return anchors;
    }

    private string BuildDetailsText(Mission mission)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Name: " + mission.missionName);
        builder.AppendLine("Customer: " + mission.customer);
        builder.AppendLine("Cargo: " + mission.cargo);

        if (string.IsNullOrEmpty(mission.description))
        {
            mission.description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";
        }

        builder.AppendLine("Description: " + mission.description);
        builder.AppendLine("Fuel cost: " + mission.fuelCost);
        builder.AppendLine("Reward: " + mission.reward);
        return builder.ToString();
    }

    public void DestroyMissionDetails()
    {
        Destroy(missionDetails);
    }
}
