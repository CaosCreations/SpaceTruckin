using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LicenceNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Licence licence;
    private Button investButton;

    public void Init(Licence licence)
    {
        this.licence = licence;
        investButton = GetComponentInChildren<Button>();
        SetInteractability();

        if (investButton.interactable)
        {
            investButton.AddOnClick(HandlePointInvestment);
        }
    }

    private bool SetInteractability()
    {
        return investButton.interactable = LicencesManager.IsTierUnlocked(licence)
            && licence.PointsInvested < licence.MaximumPoints;

    }

    private void HandlePointInvestment()
    {
        PlayerManager.Instance.InvestLicencePoint(licence);
        SetInteractability();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LicenceDetailsUI.DisplayLicenceDetails(licence);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LicenceDetailsUI.HideLicenceDetails();
    }
}
