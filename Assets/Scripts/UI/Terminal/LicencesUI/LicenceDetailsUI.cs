using System;
using System.Text;
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
        builder.AppendLineWithBreaks($"Name: {licence.Name}");
        builder.AppendLineWithBreaks($"Description: {licence.Description}");
        builder.AppendLineWithBreaks($"Effect: {GetEffectAsString(licence)}");
        builder.AppendLineWithBreaks($"Unlocked: {licence.IsUnlocked}");
        builder.AppendLineWithBreaks($"Owned: {licence.IsOwned}");

        if (licence.PrerequisiteLicence != null)
        {
            builder.AppendLineWithBreaks($"Prerequisite licence: {licence.PrerequisiteLicence.Name}");
        }
        return builder.ToString();
    }

    private static string GetEffectAsString(Licence licence)
    {
        string effectString = string.Empty;

        if (licence.Effect is MoneyEffect)
        {
            MoneyEffect moneyEffect = licence.Effect as MoneyEffect;
            effectString += $"{moneyEffect.Percentage}{LicenceConstants.MoneyEffectText}\n";
            effectString += GetEffectTotalAsString<MoneyEffect>(licence);
            
            // Other effect property values can go here
        }
        else if (licence.Effect is PilotXpEffect)
        {
            PilotXpEffect pilotXpEffect = licence.Effect as PilotXpEffect;
            effectString += $"{pilotXpEffect.Percentage}{LicenceConstants.PilotXpEffectText}\n";
            effectString += GetEffectTotalAsString<PilotXpEffect>(licence);
        }
        else if (licence.Effect is ShipDamageEffect)
        {
            ShipDamageEffect shipDamageEffect = licence.Effect as ShipDamageEffect;
            effectString += $"{shipDamageEffect.Percentage}{LicenceConstants.ShipDamageEffectText}\n";
            effectString += GetEffectTotalAsString<ShipDamageEffect>(licence);
        }
        else if (licence.Effect is FuelDiscountEffect)
        {
            FuelDiscountEffect fuelDiscountEffect = licence.Effect as FuelDiscountEffect;
            effectString += $"{fuelDiscountEffect.Percentage}{LicenceConstants.FuelDiscountEffectText}\n";
            effectString += GetEffectTotalAsString<FuelDiscountEffect>(licence);
        }
        else
        {
            Debug.Log($"Licence effect {licence.Effect} is not of a recognised effect type");
        }
        return effectString;
    }

    private static string GetEffectTotalAsString<T>(Licence licence) where T : LicenceEffect
    {
        string effectTotal;
        string effectTotalMessage;
        double currentTotalAsPercentage = LicencesManager.GetTotalEffect<T>() * 100;
        Type effectType = typeof(T);

        if (licence.IsOwned)
        {
            // Display the current total percentage effect if they already have the licence
            effectTotal = currentTotalAsPercentage.ToString();
            effectTotalMessage = LicenceConstants.CurrentTotalEffectMessage;
        }
        else
        {
            // Display what the total percentage will be after the licence is acquired
            effectTotalMessage = LicenceConstants.FutureTotalEffectMessage;

            if (effectType.IsSubclassOf(typeof(NegativeLicenceEffect)))
            {
                // Subtract the licence coefficient if it contributes to a percentage decrease
                effectTotal = (100 - currentTotalAsPercentage - licence.Effect.Percentage).ToString();
                effectTotalMessage += "reduction";

                // Todo: precalculate the subtraction from 1 (don't do it here)
            }
            else
            {
                effectTotal = (currentTotalAsPercentage + licence.Effect.Percentage).ToString();
            }
        }
        return $"{effectTotalMessage}{effectTotal}%";
    }
}
