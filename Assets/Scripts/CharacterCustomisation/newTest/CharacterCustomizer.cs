using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterCustomizer : MonoBehaviour
{
    public List<Sprite> bodySprites;
    public List<Sprite> hairSprites;
    public List<Sprite> clothesSprites;

    public Image bodyPreview;
    public Image hairPreview;
    public Image clothesPreview;

    public Button nextBody, prevBody, nextHair, prevHair, nextClothes, prevClothes;
    public Button confirmButton; // Creates the player

    private int selectedBody = 0;
    private int selectedHair = 0;
    private int selectedClothes = 0;

    private void Start()
    {
        UpdatePreview();

        nextBody.onClick.AddListener(() => ChangeSelection(ref selectedBody, bodySprites.Count));
        prevBody.onClick.AddListener(() => ChangeSelection(ref selectedBody, bodySprites.Count, false));

        nextHair.onClick.AddListener(() => ChangeSelection(ref selectedHair, hairSprites.Count));
        prevHair.onClick.AddListener(() => ChangeSelection(ref selectedHair, hairSprites.Count, false));

        nextClothes.onClick.AddListener(() => ChangeSelection(ref selectedClothes, clothesSprites.Count));
        prevClothes.onClick.AddListener(() => ChangeSelection(ref selectedClothes, clothesSprites.Count, false));

        confirmButton.onClick.AddListener(ConfirmSelection);
    }

    private void ChangeSelection(ref int index, int max, bool forward = true)
    {
        index = (index + (forward ? 1 : -1) + max) % max;
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        bodyPreview.sprite = bodySprites[selectedBody];
        hairPreview.sprite = hairSprites[selectedHair];
        clothesPreview.sprite = clothesSprites[selectedClothes];
    }

    private void ConfirmSelection()
    {
        CharacterData chosenData = new CharacterData(selectedBody, selectedHair, selectedClothes);
        PlayerSpawner.Instance.SpawnPlayer(chosenData);
        gameObject.SetActive(false); // Hide UI after selection
    }
}
