using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the presentation of details for a single recently completed mission.
/// </summary>
public class NewDayReportCard : MonoBehaviour
{
    public Image ShipAvatar;
    public Button NextCardButton;

    [SerializeField] private Text headerText;
    [SerializeField] private NewDayReportDetailsCard shipDetailsCard;
    [SerializeField] private NewDayReportDetailsCard moneyDetailsCard;
    [SerializeField] private NewDayReportDetailsCard bonusMoneyDetailsCard;
    [SerializeField] private NewDayReportDetailsCard xpDetailsCard;
    [SerializeField] private NewDayReportDetailsCard bonusXpDetailsCard;
    [SerializeField] private NewDayReportDetailsCard completedCountDetailsCard;
    [SerializeField] private NewDayReportDetailsCard levelledUpDetailsCard;
    [SerializeField] NewDayReportDetailsCard levelledUpMiniDetailsCard;
    private NewDayReportDetailsCard[] detailsCards;
    private int detailsCardIndex;
    [SerializeField] private float detailsCardDelay = 2f;

    public void Init()
    {
        detailsCards = new[]
        {
            shipDetailsCard,
            moneyDetailsCard,
            bonusMoneyDetailsCard,
            xpDetailsCard,
            bonusXpDetailsCard,
            completedCountDetailsCard,
            levelledUpDetailsCard,
            levelledUpMiniDetailsCard,
        };
        Array.ForEach(detailsCards, dc => dc.HideDetails());
    }

    public virtual void ShowReport(ArchivedMission archivedMission)
    {
        Array.ForEach(detailsCards, dc => dc.HideDetails());
        NextCardButton.SetActive(false);

        if (archivedMission == null
            || archivedMission.Pilot == null
            || archivedMission.Pilot.Ship == null
            || archivedMission.Pilot.Avatar == null)
        {
            Debug.LogError("Invalid arguments passed to ShowReport method");
            return;
        }
        detailsCardIndex = 0;
        ShipAvatar.sprite = archivedMission.Pilot.Ship.Avatar;

        var vm = new ArchivedMissionViewModel(archivedMission);
        headerText.SetText($"{vm.Pilot.Name} has returned from {vm.Mission.Name}!");
        shipDetailsCard.SetText($"{vm.Pilot.Ship.Name} has sustained <b>{vm.ShipChanges.DamageTaken} Damage</b> to its <b>Hull</b> and used up <b>{vm.ShipChanges.FuelLost} Fuel Units</b>");
        moneyDetailsCard.SetText($"Money Earned from Job: <b>R${vm.Earnings.BaseEarnings}</b>");
        bonusMoneyDetailsCard.SetText($"Bonus Earnings: <b>R${vm.Earnings.BonusesEarnings}</b>");
        xpDetailsCard.SetText($"{vm.Pilot.Name} has gained <b>{vm.XpGains.BaseXpGain}EXP</b>");
        bonusXpDetailsCard.SetText($"Bonus EXP: <b>{vm.XpGains.BonusesXpGain}</b>");
        completedCountDetailsCard.SetText($"<b>{vm.Pilot.Name} has completed {vm.Pilot.MissionsCompleted} Jobs!</b>");
        levelledUpDetailsCard.SetText("Levelled Up!");
        levelledUpMiniDetailsCard.SetText("Levelled Up!");
        StartCoroutine(ShowDetailsCards(vm));
    }

    private IEnumerator ShowDetailsCards(ArchivedMissionViewModel vm)
    {
        while (detailsCardIndex < detailsCards.Length)
        {
            float elapsedTime = 0;
            bool cardShown = false;

            while (elapsedTime < detailsCardDelay && !cardShown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    cardShown = true;
                    ShowNextDetailsCard(vm.LevelledUp);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // If the card was not shown by the mouse click, show it after the delay
            if (!cardShown)
                ShowNextDetailsCard(vm.LevelledUp);
        }
    }

    private void ShowNextDetailsCard(bool levelledUp)
    {
        if (detailsCardIndex >= detailsCards.Length)
            return;

        var nextCard = detailsCards[detailsCardIndex];
        if (!((nextCard == levelledUpDetailsCard || nextCard == levelledUpMiniDetailsCard) && !levelledUp))
        {
            nextCard.ShowDetails();
        }
        detailsCardIndex++;

        if (detailsCardIndex == detailsCards.Length - 1)
            NextCardButton.SetActive(true);
    }
}
