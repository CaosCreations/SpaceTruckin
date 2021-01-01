using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionDetailsUI : MonoBehaviour
{
    public GameObject missionDetails;
    public Mission missionBeingDisplayed;

    public void DisplayMissionDetails(MissionUIItem listItem)
    {
        DestroyMissionDetails();
        SetupDetailsContainer(listItem.GetComponent<RectTransform>());
        SetupDetailsText();
    }

    public void DestroyMissionDetails()
    {
        Destroy(missionDetails);
    }

    private void SetupDetailsContainer(RectTransform listItemRect)
    {
        missionDetails = new GameObject("MissionDetails");
        missionDetails.transform.parent = transform;
        RectTransform detailsRect = missionDetails.AddComponent<RectTransform>();
        detailsRect.Reset();
        detailsRect.SetAnchors(GetMissionDetailsAnchors(listItemRect));
        missionDetails.AddComponent<Image>().color = Color.magenta;

        // Let the player close the mission details box by clicking on it.
        missionDetails.AddCustomEvent(EventTriggerType.PointerClick, () => DestroyMissionDetails());
    }

    /// <summary>
    ///     Returns a tuple of anchors based on the relative position of the item in the list of missions to the view.
    ///     e.g. If the item is in the top third of the list, the anchors will span the top third of the view.
    ///     This is so the mission details will be in line with the corresponding mission UI item.
    /// </summary>
    /// <param name="listItemRect"></param>
    /// <returns>Anchor min and anchor max</returns>
    private ValueTuple<Vector2, Vector2> GetMissionDetailsAnchors(RectTransform listItemRect)
    {
        var anchors = new ValueTuple<Vector2, Vector2>();
        anchors.Item1.x = 0.55f;
        anchors.Item2.x = 0.95f;

        // Top third 
        if (listItemRect.position.y > Screen.height / 1.5)
        {
            anchors.Item1.y = 0.65f;
            anchors.Item2.y = 0.95f;
        }
        // Middle third 
        else if (listItemRect.position.y >= Screen.height / 3 && listItemRect.position.y <= Screen.height / 1.5)
        {
            anchors.Item1.y = 0.35f;
            anchors.Item2.y = 0.65f;
        }
        // Bottom third 
        else
        {
            anchors.Item1.y = 0.05f;
            anchors.Item2.y = 0.35f;
        }
        return anchors;
    }

    private void SetupDetailsText()
    {
        GameObject textContainer = new GameObject("MissionDetailsText");
        textContainer.transform.parent = missionDetails.transform;
        Text detailsText = textContainer.AddComponent<Text>();
        RectTransform textRect = textContainer.GetComponent<RectTransform>();
        textRect.Reset();
        textRect.Stretch();
        detailsText.text = BuildDetailsString(missionBeingDisplayed);
        detailsText.SetDefaultFont();
        detailsText.resizeTextForBestFit = true;
        detailsText.color = Color.black;
    }

    public string BuildDetailsString(Mission mission)
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
}

