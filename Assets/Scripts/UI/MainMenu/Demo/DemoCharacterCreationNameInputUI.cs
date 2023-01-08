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
            StartCoroutine(WaitForValueUpdated());
            if (PlayerManager.Instance == null)
            {
                throw new System.Exception("PlayerManager is null. Cannot set name");
            }
            PlayerManager.SetPlayerName(CharacterName);
        });

        static IEnumerator WaitForValueUpdated()
        {
            yield return new WaitForSeconds(UIConstants.PlayerNameSettingsDelayInSeconds);
        }
    }

    // Temporary demo limitation as we do not have confirmation button when setting name 
    protected override void Update()
    {
        return;
    }
}