using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionDetailsUI : MonoBehaviour
{
    public GameObject missionDetails;
    public Mission missionBeingDisplayed;
    private MissionsUI missionsUI;

    private void Start()
    {
        missionsUI = GetComponent<MissionsUI>();
    }

    public void DisplayMissionDetails(MissionUIItem listItem)
    {
        DestroyMissionDetails();

        // Let the player close the mission details box by clicking on it 
        missionDetails.AddCustomEvent(EventTriggerType.PointerClick, () => DestroyMissionDetails());
        SetupDetailsContainer(listItem.GetComponent<RectTransform>());
        SetupDetailsText();
    }

    public void DestroyMissionDetails()
    {
        Destroy(missionDetails);
    }

    private void SetupDetailsContainer(RectTransform listRect)
    {
        missionDetails = new GameObject("MissionDetails");
        missionDetails.transform.parent = transform;
        RectTransform detailsRect = missionDetails.AddComponent<RectTransform>();
        detailsRect.ResetRect();
        detailsRect.SetAnchors(GetMissionDetailsAnchors(listRect));
        missionDetails.AddComponent<Image>().color = Color.magenta;
    }

    private void SetupDetailsText()
    {
        GameObject textContainer = new GameObject("MissionDetailsText");
        textContainer.transform.parent = missionDetails.transform;
        Text detailsText = textContainer.AddComponent<Text>();
        RectTransform textRect = textContainer.GetComponent<RectTransform>();
        textRect.ResetRect();
        textRect.StretchAnchors();
        detailsText.text = BuildDetailsString();
        detailsText.SetDefaultFont();
        detailsText.resizeTextForBestFit = true;
        detailsText.color = Color.black;
    }

    /// <summary>
    ///     Returns a tuple of anchors based on the local position of the item in the list of missions.
    ///     e.g. If the item is in the top third of the list, the anchors will span the top third of the view.
    ///     This is so the mission details will be in line with the corresponding mission UI item
    /// </summary>
    /// <param name="listRect"></param>
    /// <returns></returns>
    private ValueTuple<Vector2, Vector2> GetMissionDetailsAnchors(RectTransform listRect)
    {
        var anchors = new ValueTuple<Vector2, Vector2>();
        anchors.Item1.x = 0.55f;
        anchors.Item2.x = 0.95f;

        // Top third 
        if (listRect.localPosition.y < 0.33f)
        {
            anchors.Item1.y = 0.65f;
            anchors.Item2.y = 0.95f;
        }
        // Middle third 
        else if (listRect.localPosition.y >= 0.33f && listRect.localPosition.y < 0.66f)
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

    private string BuildDetailsString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Name: " + missionBeingDisplayed.missionName);
        builder.AppendLine("Customer: " + missionBeingDisplayed.customer);
        builder.AppendLine("Cargo: " + missionBeingDisplayed.cargo);

        if (string.IsNullOrEmpty(missionBeingDisplayed.description))
        {
            missionBeingDisplayed.description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";
        }

        builder.AppendLine("Description: " + missionBeingDisplayed.description);
        builder.AppendLine("Fuel cost: " + missionBeingDisplayed.fuelCost);
        builder.AppendLine("Reward: " + missionBeingDisplayed.reward);
        return builder.ToString();
    }
}

