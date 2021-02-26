using UnityEngine;
using UnityEngine.EventSystems;

public class LicenceNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Licence licence;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LicenceDetailsUI.DisplayLicenceDetails(licence);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LicenceDetailsUI.HideLicenceDetails();
    }
}
