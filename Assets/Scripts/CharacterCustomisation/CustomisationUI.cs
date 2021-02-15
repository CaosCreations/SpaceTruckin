using UnityEngine;
using UnityEngine.UI;

public class CustomisationUI : MonoBehaviour
{
    public GameObject customisationCanvas;
    public GameObject saveButtonPrefab;
    private ColourSliders colourSliders;

    private void Start()
    {
        colourSliders = GetComponent<ColourSliders>();
        colourSliders.Init(PlayerManager.Instance.SpriteColour);
        InitSaveButton();
    }
    
    private void OnEnable()
    {

    }

    //private void OnDisable()
    //{
    //    PlayerManager.Instance.SetSpriteColour(colourSliders.sliderImage.color);
    //}

    private void SaveChanges()
    {
        PlayerManager.Instance.SetSpriteColour(colourSliders.sliderImage.color);
    }

    private void InitSaveButton()
    {
        GameObject buttonObject = Instantiate(saveButtonPrefab, customisationCanvas.transform);
        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.SetAnchors(CustomisationConstants.saveButtonAnchors);

        Button saveButton = buttonObject.GetComponent<Button>();
        saveButton.AddOnClick(SaveChanges);
        saveButton.SetText(CustomisationConstants.saveButtonText);

        //GameObject buttonObject = new GameObject().ScaffoldUI(
        //    name: "SaveButton", parent: customisationCanvas, 
        //    anchors: CustomisationConstants.saveButtonAnchors);

        //Instantiate(saveButtonPrefab, customisationCanvas.transform)
        //    .GetComponent<>
        //    .GetComponent<Button>()
        //    .AddOnClick(SaveCustomisation);

        //Button saveButton = buttonObject.GetComponent<Button>().AddOnClick(SaveCustomisation);
        //GameObject buttonObject = Instantiate(saveButtonPrefab, customisationCanvas.transform);
        //Button saveButton = buttonObject.GetComponent<Button>().AddOnClick(SaveCustomisation);

    }
}
