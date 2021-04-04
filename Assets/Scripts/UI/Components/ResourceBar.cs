using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Text currentPercentageText;
    private float currentPercentage;

    [SerializeField] private Slider resourceSlider;
    [SerializeField] private Image sliderBackgroundImage; 

    private Gradient gradient;
    private GradientColorKey[] colourKeys;
    private GradientAlphaKey[] alphaKeys;

    private void Awake()
    {
        InitGradient();
    }

    private void InitGradient()
    {
        // Colour 
        colourKeys = new GradientColorKey[2];
        colourKeys[0].color = UIConstants.Matrix; // Red 
        colourKeys[1].color = UIConstants.ChelseaCucumber; // Green 
        colourKeys[0].time = 0f;
        colourKeys[1].time = 1f;

        // Alpha 
        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1f;
        alphaKeys[1].alpha = 0f;
        alphaKeys[0].time = 1f;
        alphaKeys[1].time = 0f;

        gradient = new Gradient();
        gradient.SetKeys(colourKeys, alphaKeys);
    }

    public void SetResourceValue(float value)
    {
        currentPercentage = value;
        currentPercentageText.SetText($"{Mathf.RoundToInt(currentPercentage * 100)}%");
        resourceSlider.value = currentPercentage;
        SetBarColour();
    }

    public void SetBarColour()
    {
        Color newColour = GetColourFromGradient(currentPercentage);
        sliderBackgroundImage.color = newColour;
    }

    public Color GetColourFromGradient(float value)
    {
        return gradient.Evaluate(value);
    }
}
