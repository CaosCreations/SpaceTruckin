using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionDetailsUI : MonoBehaviour
{
    public GameObject missionDetailsPrefab;
    public GameObject missionDetails;
    public Mission missionBeingDisplayed;

    public void DisplayMissionDetails(MissionUIItem listItem)
    {
        DestroyMissionDetails();
        InitMissionDetails(listItem.GetComponent<RectTransform>());
    }

    public void DestroyMissionDetails()
    {
        missionDetails.DestroyIfExists();
    }

    private void InitMissionDetails(RectTransform listItemRect)
    {
        missionDetails = Instantiate(missionDetailsPrefab, transform);
        missionDetails.name = "MissionDetails";

        Text detailsText = missionDetails.GetComponentInChildren<Text>();
        detailsText.SetText(BuildDetailsString(missionBeingDisplayed));

        RectTransform detailsRect = missionDetails.GetComponent<RectTransform>();
        detailsRect.SetAnchors(GetMissionDetailsAnchors(listItemRect));

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

    public string BuildDetailsString(Mission mission)
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLineWithBreaks("Name: " + mission.Name.ToItalics());
        builder.AppendLineWithBreaks("Customer: " + mission.Customer.ToItalics());
        builder.AppendLineWithBreaks("Cargo: " + mission.Cargo.ToItalics());

        if (string.IsNullOrEmpty(mission.Description))
        {
            mission.Description = LoremIpsumGenerator.GenerateLoremIpsum();
        }

        builder.AppendLineWithBreaks("Description: " + mission.Description.ToItalics());
        builder.AppendLineWithBreaks("Fuel cost: " + mission.FuelCost.ToString().ToItalics());
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

        if (moneyOutcome != null)
        {
            builder.AppendLineWithBreaks("Money: " + $"${moneyOutcome.MoneyMin} - {moneyOutcome.MoneyMax}".ToItalics());
        }

        PilotXpOutcome pilotXpOutcome = MissionUtils.GetOutcomeByType<PilotXpOutcome>(mission.Outcomes);

        if (pilotXpOutcome != null)
        {
            builder.AppendLineWithBreaks("Pilot xp: " + $"{pilotXpOutcome.XpMin} - {pilotXpOutcome.XpMax}".ToItalics());
        }

        return builder.ToString();
    }
}
