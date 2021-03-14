﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LicenceDetailsUI : MonoBehaviour
{
    public GameObject licenceDetailsPrefab;
    private static GameObject licenceDetails;
    private static Text detailsText;

    private void Start()
    {
        licenceDetails = Instantiate(licenceDetailsPrefab, transform);
        HideLicenceDetails();
        detailsText = licenceDetails.GetComponentInChildren<Text>();
    }

    public static void HideLicenceDetails()
    {
        licenceDetails.SetActive(false);
    }

    public static void DisplayLicenceDetails(Licence licence)
    {
        licenceDetails.SetActive(true);
        detailsText.SetText(BuildDetailsString(licence));
    }

    private static string BuildDetailsString(Licence licence)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks($"Name: {licence.Name.ToItalics()}");
        builder.AppendLineWithBreaks($"Description: {licence.Description.ToItalics()}");
        builder.AppendLineWithBreaks($"Effect: {GetEffectString(licence).ToItalics()}");
        builder.AppendLineWithBreaks($"Unlocked: {licence.IsUnlocked.ToString().ToItalics()}");
        builder.AppendLineWithBreaks($"Owned: {licence.IsOwned.ToString().ToItalics()}");

        if (licence.PrerequisiteLicence != null)
        {
            builder.AppendLineWithBreaks($"Prerequisite licence: {licence.PrerequisiteLicence.Name.ToItalics()}");
        }
        return builder.ToString();
    }

    private static string GetEffectString(Licence licence)
    {
        string effectString = string.Empty;

        if (licence.Effect is MoneyEffect)
        {
            MoneyEffect moneyEffect = licence.Effect as MoneyEffect;
            effectString += $"{moneyEffect.Percentage}{LicenceConstants.MoneyEffectText}\n";
            effectString += GetTotalEffectString<MoneyEffect>(licence);
            
            // Other effect property values can go here
        }
        else if (licence.Effect is PilotXpEffect)
        {
            PilotXpEffect pilotXpEffect = licence.Effect as PilotXpEffect;
            effectString += $"{pilotXpEffect.Percentage}{LicenceConstants.PilotXpEffectText}\n";
            effectString += GetTotalEffectString<PilotXpEffect>(licence);
        }
        else if (licence.Effect is ShipDamageEffect)
        {
            ShipDamageEffect shipDamageEffect = licence.Effect as ShipDamageEffect;
            effectString += $"{shipDamageEffect.Percentage}{LicenceConstants.ShipDamageEffectText}\n";
            effectString += GetTotalEffectString<ShipDamageEffect>(licence);
        }
        else if (licence.Effect is FuelDiscountEffect)
        {
            FuelDiscountEffect fuelDiscountEffect = licence.Effect as FuelDiscountEffect;
            effectString += $"{fuelDiscountEffect.Percentage}{LicenceConstants.FuelDiscountEffectText}\n";
            effectString += GetTotalEffectString<FuelDiscountEffect>(licence);
        }
        else
        {
            Debug.Log($"Licence effect {licence.Effect} is not of a recognised effect type");
        }
        return effectString;
    }

    // Separate method for unlock licences?
    private static string GetTotalEffectString<T>(Licence licence) where T : PercentageEffect
    {
        double totalPercentage = LicencesManager.GetTotalPercentageEffect<T>();
        string totalEffectMessage;
        if (licence.IsOwned)
        {
            totalEffectMessage = LicenceConstants.CurrentTotalEffectMessage;
        }
        else
        {
            totalEffectMessage = LicenceConstants.FutureTotalEffectMessage;
            if (licence.Effect is PercentageEffect)
            {
                Debug.Log("Effect is % effect");
                PercentageEffect percentageEffect = licence.Effect as PercentageEffect;
                totalPercentage += percentageEffect.Percentage;
            }
            
        }
        totalEffectMessage += totalPercentage.ToString() + "%";

        if (typeof(T).IsSubclassOf(typeof(PercentageDecreaseEffect)))
        {
            totalEffectMessage += " reduction";
        }
        else
        {
            totalEffectMessage += " increase";
        }
        return totalEffectMessage;
    }
}
