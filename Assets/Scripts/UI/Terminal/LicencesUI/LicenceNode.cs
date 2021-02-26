﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LicenceNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Licence licence;
    private Button nodeButton;

    public void Init(Licence licence)
    {
        this.licence = licence;
        nodeButton = GetComponent<Button>();
        SetInteractability();

        if (nodeButton.interactable)
        {
            nodeButton.AddOnClick(HandleLicenceAcquisition);
        }
    }

    private bool SetInteractability()
    {
        bool interactable = true;
        if (licence.PrerequisiteLicence != null)
        {
            // Unless the licence has no prerequisite (is on tier 1),
            // the prerequisite licence must be owned in order to get the licence.
            interactable = licence.PrerequisiteLicence.IsOwned;
        }
        // Grey out licences that are locked or have already been purchased.
        return interactable && licence.IsUnlocked && !licence.IsOwned;
    }

    private void HandleLicenceAcquisition()
    {
        PlayerManager.Instance.AcquireLicence(licence);
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
