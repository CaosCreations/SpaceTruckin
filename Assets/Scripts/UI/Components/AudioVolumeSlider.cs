using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    private Text sliderText;

    [SerializeField] private MixerGroup mixerGroup;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.AddOnValueChanged(SlideVolume);

        sliderText = GetComponent<Text>();
        sliderText.SetText(Enum.GetName(typeof(MixerGroup), mixerGroup));
    }

    private void SlideVolume()
    {
        AudioMixerManager.Instance.SetMixerGroupVolume(mixerGroup, volumeSlider.value);
    }
}
