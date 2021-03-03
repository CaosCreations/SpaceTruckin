using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LicenceNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Licence licence;
    private Button nodeButton;
    public static event Action OnLicenceAcquisition;

    public void Init(Licence licence)
    {
        this.licence = licence;
        GetComponentInChildren<Text>().SetText(licence.PointsCost.ToString());
        nodeButton = GetComponent<Button>();

        SetInteractability();
        OnLicenceAcquisition += SetInteractability;
        nodeButton.AddOnClick(HandleLicenceAcquisition);
    }

    private void SetInteractability()
    {
        bool interactable = true;
        if (licence.AntecedentLicences != null)
        {
            interactable = licence.AntecedentLicences.Any(x => x.IsOwned);
        }
        nodeButton.interactable = interactable && licence.IsUnlocked && !licence.IsOwned;
    }

    private void HandleLicenceAcquisition()
    {
        PlayerManager.Instance.AcquireLicence(licence);
        LicenceDetailsUI.DisplayLicenceDetails(licence);
        OnLicenceAcquisition?.Invoke();
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
