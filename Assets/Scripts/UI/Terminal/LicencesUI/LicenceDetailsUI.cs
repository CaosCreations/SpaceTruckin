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
        builder.AppendLineWithBreaks($"Name: {licence.licenceName}");
        builder.AppendLineWithBreaks($"Description: {licence.description}");
        builder.AppendLineWithBreaks($"Points invested: {licence.pointsInvested} out of {licence.maximumPoints}");
        
        return builder.ToString();
    }
}
