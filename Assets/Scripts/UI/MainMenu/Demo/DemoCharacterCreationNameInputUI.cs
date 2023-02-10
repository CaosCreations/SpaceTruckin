// This inheritance is bad but temporary 
using System.Collections;
using UnityEngine;

public class DemoCharacterCreationNameInputUI : CharacterCreationUI
{
    protected override void AddListeners()
    {
        characterNameInput.onValueChanged.RemoveAllListeners();

        AddValidationListener();
        AddFormattingListener();
        AddNameSettingListener();
    }

    private void AddNameSettingListener()
    {
        characterNameInput.AddOnValueChanged(() =>
        {
            StartCoroutine(WaitAndSetName());
        });

        IEnumerator WaitAndSetName()
        {
            yield return new WaitForSeconds(UIConstants.PlayerNameSettingsDelayInSeconds);
            ChooseName();
        }
    }

    // Temporary demo limitation as we do not have confirmation button when setting name 
    protected override void Update()
    {
        return;
    }
}