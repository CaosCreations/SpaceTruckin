using UnityEngine;

public class CustomisationUI : MonoBehaviour
{
    public GameObject customisationCanvas;
    public ColourSliders colourSliders;

    private void Start()
    {
        colourSliders.Init(PlayerManager.Instance.SpriteColour);
    }

    private void OnDisable()
    {
        PlayerManager.Instance.SetSpriteColour(colourSliders.sliderImage.color);
    }
}
