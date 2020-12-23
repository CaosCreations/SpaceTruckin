using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject missionDetailsPrefab;
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

    public void DisplayMissionDetails(MissionUIItem listItem)
    {
        GameObject missionDetails = new GameObject("MissionDetails");
        missionDetails.transform.parent = transform;
        //missionDetails.transform.parent = missionSchedule.transform;
        //missionDetails.AddComponent<Image>().color = Color.magenta;
        RectTransform listRect = listItem.GetComponent<RectTransform>();
        RectTransform detailsRect = missionDetails.AddComponent<RectTransform>();
        detailsRect.ResetRect();

        var anchors = new System.ValueTuple<Vector2, Vector2>();
        anchors.Item1.x = 0.55f;

        // Place the details in line with the corresponding mission
        if (listRect.localPosition.y < 0.33f)
        {
            anchors.Item1.y = 0.66f;
            anchors.Item2 = Vector2.one;
        }
        else if (listRect.localPosition.y >= 0.33f && listRect.localPosition.y < 0.66f)
        {
            anchors.Item1.y = 0.33f;
            anchors.Item2 = new Vector2(1f, 0.66f);
        }
        else
        {
            anchors.Item1.y = 0.33f;
            anchors.Item2 = Vector2.right;
        }
        detailsRect.SetAnchors(anchors);

        Text detailsText = missionDetails.AddComponent<Text>();
        //GameObject textContainer = new GameObject("MissionDetailsText");
        //textContainer.transform.parent = missionDetails.transform;
        //Text detailsText = textContainer.AddComponent<Text>();
        //textContainer.GetComponent<RectTransform>().ResetRect();
        detailsText.text = BuildDetailsText(listItem.mission);
        detailsText.SetDefaultFont();
        detailsText.fontSize = 36;

    }

    private string BuildDetailsText(Mission mission)
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
