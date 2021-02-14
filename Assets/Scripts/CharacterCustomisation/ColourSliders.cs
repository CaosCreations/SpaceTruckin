using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Channel { Red = 0, Green = 1, Blue = 2 }; 

public class ColourSliders : MonoBehaviour
{
    public GameObject colourSliderPrefab; 
    private GameObject sliderContainer;
    public Image sliderImage; 
    private GameObject redSlider;
    private GameObject greenSlider;
    private GameObject blueSlider;

    public void Init(Color startingColour)
    {
        sliderContainer = InitSliderContainer();
        sliderImage = InitImage(startingColour);
        redSlider = InitSlider(Channel.Red);
        greenSlider = InitSlider(Channel.Green);
        blueSlider = InitSlider(Channel.Blue);
    }

    private GameObject InitSliderContainer()
    {
        GameObject sliderContainer = new GameObject("SliderContainer");
        sliderContainer.transform.parent = transform;
        sliderContainer.transform.localPosition = Vector2.zero;
        sliderContainer.AddComponent<VerticalLayoutGroup>();
        return sliderContainer;
    }

    private Image InitImage(Color startingColour)
    {
        GameObject imageObject = new GameObject("SliderImage");
        imageObject.transform.parent = sliderContainer.transform;
        Image sliderImage = imageObject.AddComponent<Image>();
        sliderImage.color = startingColour;
        return sliderImage;
    }

    private GameObject InitSlider(Channel channel)
    {
        GameObject sliderObject = Instantiate(colourSliderPrefab);  
        sliderObject.transform.parent = sliderContainer.transform; 

        Slider slider = sliderObject.GetComponent<Slider>();
        slider.minValue = 0f; 
        slider.maxValue = 255f;
        slider.onValueChanged.AddListener(delegate { UpdateImageColour(channel, slider.value); });

        return sliderObject;
    }

    private void UpdateImageColour(Channel channel, float value)
    {
        switch (channel)
        {
            case Channel.Red:
                // Divide by 255 to normalise the value 
                sliderImage.color = new Color(value / 255f, sliderImage.color.g, sliderImage.color.b);
                break;
            case Channel.Green:
                sliderImage.color = new Color(sliderImage.color.r, value / 255f, sliderImage.color.b);
                break;
            case Channel.Blue:
                sliderImage.color = new Color(sliderImage.color.r, sliderImage.color.g, value / 225f);
                break; 
        }
    }

    private void LogSliderValues()
    {
        Debug.Log("Red slider value: " + redSlider.GetComponent<Slider>().value);
        Debug.Log("Green slider value: " + greenSlider.GetComponent<Slider>().value);
        Debug.Log("Blue slider value: " + blueSlider.GetComponent<Slider>().value);
    }
}
