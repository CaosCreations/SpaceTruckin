using System;
using UnityEngine;

public class DemoCharacterPicker : DemoSpritePicker
{
    [SerializeField]
    private Character char1;
    [SerializeField]
    private Character char2;

    public Character SelectedCharacter;

    protected override void Start()
    {
        base.Start();
        UpdatePlayerData(char1);
    }

    public override void PickSprite()
    {
        base.PickSprite();

        if (charSprite1.activeSelf)
            SelectedCharacter = char1;
        else if (charSprite2.activeSelf)
            SelectedCharacter = char2;
        else
        {
            Debug.LogError("No sprites are active.");
            return;
        }

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("PlayerManager instance is null. Cannot set selected player data.");
            return;
        }

        if (SelectedCharacter == null)
        {
            Debug.LogError("Selected character was null. Cannot set selected player data.");
            return;
        }

        UpdatePlayerData(SelectedCharacter);
    }

    protected void UpdatePlayerData(Character character)
    {
        if (character == null)
            throw new NullReferenceException("Character null. Cannot pick character and update data.");

        // Set the prefab and animator settings to use in-game based on input 
        PlayerManager.Instance.PlayerPrefab = character.Prefab;
        PlayerManager.Instance.PlayerAnimatorSettings = character.AnimatorSettings;
    }
}