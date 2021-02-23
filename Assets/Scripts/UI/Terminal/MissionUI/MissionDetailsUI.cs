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
        detailsText.SetText(BuildDetailsString(missionBeingDisplayed), FontType.Paragraph);
        detailsText.resizeTextForBestFit = true;
        detailsText.color = Color.black;
    }

    public string BuildDetailsString(Mission mission)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks("Name: " + mission.Name);
        builder.AppendLineWithBreaks("Customer: " + mission.Customer);
        builder.AppendLineWithBreaks("Cargo: " + mission.Cargo);

        if (string.IsNullOrEmpty(mission.Description))
        {
            mission.Description = PlaceholderUtils.GenerateLoremIpsum();
        }

        builder.AppendLineWithBreaks("Description: " + mission.Description);
        builder.AppendLineWithBreaks("Fuel cost: " + mission.FuelCost);
        builder.AppendLineWithBreaks("Reward: " + BuildRewardString(mission));
        return builder.ToString();
    }

    public static string BuildRewardString(Mission mission)
    {
        StringBuilder builder = new StringBuilder();
        if (mission.HasRandomOutcomes)
        {
            return builder.Append("???").ToString();
        }

        MoneyOutcome moneyOutcome = MissionUtils.GetOutcomeByType<MoneyOutcome>(mission.Outcomes);
        PilotXpOutcome pilotXpOutcome = MissionUtils.GetOutcomeByType<PilotXpOutcome>(mission.Outcomes);
        if (moneyOutcome != null)
        {
            builder.AppendLineWithBreaks($"Money: ${moneyOutcome.MoneyMin} - {moneyOutcome.MoneyMax}");
        }
        if (pilotXpOutcome != null)
        {
            builder.AppendLineWithBreaks($"Pilot xp: {pilotXpOutcome.XpMin} - {pilotXpOutcome.XpMax}");
        }
        return builder.ToString();
    }
}
