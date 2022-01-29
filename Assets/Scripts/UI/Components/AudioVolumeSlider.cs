﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    private Text sliderText;

    [SerializeField] private MixerGroup mixerGroup;

    private void Awake()
    {
        InitSlider();
        InitSliderText();
    }

    private void InitSlider()
    {
        volumeSlider = GetComponentInChildren<Slider>();
        volumeSlider.value = UIConstants.DefaultVolumeSliderValue;
        volumeSlider.AddOnValueChanged(SlideVolume);
    }

    private void InitSliderText()
    {
        sliderText = GetComponentInChildren<Text>();
        sliderText.SetText(Enum.GetName(typeof(MixerGroup), mixerGroup));
    }

    private void SlideVolume()
    {
        AudioMixerManager.Instance.SetMixerGroupVolume(mixerGroup, volumeSlider.value);
    }
}