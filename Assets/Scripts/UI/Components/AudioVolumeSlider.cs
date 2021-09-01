using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    private Text sliderText;

    [SerializeField] private MixerGroup mixerGroup;

    private void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
        volumeSlider.AddOnValueChanged(SlideVolume);

        sliderText = GetComponentInChildren<Text>();
        sliderText.SetText(Enum.GetName(typeof(MixerGroup), mixerGroup));
    }

    private void SlideVolume()
    {
        AudioMixerManager.Instance.SetMixerGroupVolume(mixerGroup, volumeSlider.value);
    }
}
