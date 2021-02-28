using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LicenceNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Licence licence;
    private Text nodeText;
    private Button nodeButton;
    public static event Action OnLicenceAcquisition;

    public void Init(Licence licence)
    {
        this.licence = licence;
        nodeText = GetComponentInChildren<Text>().SetText(licence.PointsCost.ToString());
        nodeButton = GetComponent<Button>();

        SetInteractability();
        OnLicenceAcquisition += SetInteractability;
        nodeButton.AddOnClick(HandleLicenceAcquisition);
    }

    private void SetInteractability()
    {
        bool interactable = true;
        if (licence.PrerequisiteLicence != null)
        {
            interactable = licence.PrerequisiteLicence.IsOwned;
        }
        nodeButton.interactable = interactable && licence.IsUnlocked && !licence.IsOwned;
    }

    private void HandleLicenceAcquisition()
    {
        PlayerManager.Instance.AcquireLicence(licence);
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
