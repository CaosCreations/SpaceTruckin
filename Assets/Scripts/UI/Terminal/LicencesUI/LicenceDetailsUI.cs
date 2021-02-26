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
        builder.AppendLineWithBreaks($"Effect: {GetEffectDetails(licence.Effect)}");
        builder.AppendLineWithBreaks($"Owned? {licence.IsOwned}");
        
        return builder.ToString();
    }

    private static string GetEffectDetails(LicenceEffect effect)
    {
        string effectDetails = string.Empty;
        if (effect is MoneyEffect)
        {
            MoneyEffect moneyEffect = effect as MoneyEffect;
            effectDetails = $"{LicenceUtils.CoefficientToPercentage(moneyEffect.effect)}% increased money from missions";
        }
        else if (effect is PilotXpEffect)
        {
            PilotXpEffect pilotXpEffect = effect as PilotXpEffect;
            effectDetails = $"{LicenceUtils.CoefficientToPercentage(pilotXpEffect.effect)}% increased pilot XP from missions";
        }
        else if (effect is ShipDamageEffect)
        {
            ShipDamageEffect shipDamageEffect = effect as ShipDamageEffect;
            effectDetails = $"{LicenceUtils.CoefficientToPercentage(shipDamageEffect.effect)}";
        }
        else if (effect is FuelDiscountEffect)
        {
            FuelDiscountEffect fuelDiscountEffect = effect as FuelDiscountEffect;
            effectDetails = $"{LicenceUtils.CoefficientToPercentage(fuelDiscountEffect.effect)}";
        }
        else
        {
            Debug.Log($"Licence effect {effect} is not of a recognised effect type");
        }
        return effectDetails;
    }
}
