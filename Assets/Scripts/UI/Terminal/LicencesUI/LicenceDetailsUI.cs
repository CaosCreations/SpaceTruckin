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
        builder.AppendLineWithBreaks($"Effect: {GetEffectString(licence)}");
        builder.AppendLineWithBreaks($"Unlocked? {licence.IsUnlocked}");
        builder.AppendLineWithBreaks($"Owned? {licence.IsOwned}");
        
        return builder.ToString();
    }

    private static string GetEffectString(Licence licence)
    {
        string effectString = string.Empty;

        if (licence.Effect is MoneyEffect)
        {
            MoneyEffect moneyEffect = licence.Effect as MoneyEffect;
            effectString += $"{moneyEffect.Percentage}% increased money from missions\n";
            effectString += GetAggregateEffectString<MoneyEffect>(licence.IsOwned);
            
            // Other effect property values can go here
        }
        else if (licence.Effect is PilotXpEffect)
        {
            PilotXpEffect pilotXpEffect = licence.Effect as PilotXpEffect;
            effectString += $"{pilotXpEffect.Percentage}% increased pilot XP from missions";
            effectString += GetAggregateEffectString<PilotXpEffect>(licence.IsOwned);
        }
        else if (licence.Effect is ShipDamageEffect)
        {
            ShipDamageEffect shipDamageEffect = licence.Effect as ShipDamageEffect;
            effectString += $"{shipDamageEffect.Percentage}% reduced damage to ships";
            effectString += GetAggregateEffectString<ShipDamageEffect>(licence.IsOwned);
        }
        else if (licence.Effect is FuelDiscountEffect)
        {
            FuelDiscountEffect fuelDiscountEffect = licence.Effect as FuelDiscountEffect;
            effectString += $"{fuelDiscountEffect.Percentage}% discount on fuel";
            effectString += GetAggregateEffectString<FuelDiscountEffect>(licence.IsOwned);
        }
        else
        {
            Debug.Log($"Licence effect {licence.Effect} is not of a recognised effect type");
        }
        return effectString;
    }

    private static string GetAggregateEffectString<T>(bool isOwned) where T : LicenceEffect
    {
        string substring = isOwned ? LicenceConstants.CurrentAggregateEffectSubstring 
            : LicenceConstants.FutureAggregateEffectSubstring;

        double aggregateEffect = LicencesManager.GetAggregateEffect<T>();
        return $"{substring} {aggregateEffect}%";
    }
}
