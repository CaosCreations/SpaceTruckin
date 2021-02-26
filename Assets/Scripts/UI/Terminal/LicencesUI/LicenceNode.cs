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

        // Get button 
        // Has player unlocked the licence's tier - make interactable 
        
        // Add listener - invest point, update ui, etc.

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
