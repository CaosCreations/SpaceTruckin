using UnityEngine;

public class DemoCharacterPicker : DemoSpritePicker
{
    [SerializeField]
    private Character char1;
    [SerializeField]
    private Character char2;

    public Character SelectedCharacter;

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

        // Set the prefab to use in-game based on input 
        PlayerManager.Instance.PlayerPrefab = SelectedCharacter.Prefab;
    }
}