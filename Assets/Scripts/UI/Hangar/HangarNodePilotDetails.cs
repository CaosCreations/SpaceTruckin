using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HangarNodePilotDetails : MonoBehaviour
{
    [SerializeField]
    private Image pilotImage;

    [SerializeField]
    private Text detailsText;

    public void Init(Pilot pilot)
    {
        pilotImage.sprite = pilot.Avatar;

        detailsText.SetText(new StringBuilder()
            .AppendLine($"Pilot: {pilot.Name}")
            .AppendLine($"Ship: {pilot.Ship.Name}")
            .ToString(), FontType.Paragraph);
    }
}