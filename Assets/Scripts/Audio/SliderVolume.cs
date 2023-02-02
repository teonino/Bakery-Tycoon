using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SliderVolume : MonoBehaviour
{

    enum MixerGroup
    {
        Music,
        SFX,
        Environment
    };

    private Slider volumeSlider;
    [SerializeField] private MixerGroup groupName;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle toggleButton;
    [SerializeField] private float currentVolumeLevel;
    private float modificationMultiplier = 30f;

    private void Awake()
    {
        volumeSlider = this.transform.GetComponentInChildren<Slider>();
        audioMixer.GetFloat(groupName.ToString(), out float volume);
        volumeSlider.value = volume + 1;
        currentVolumeLevel = volumeSlider.value;
    }

    public void HandleToggleValueChanged(bool enableSound)
    {
        if (enableSound)
        {
            if(currentVolumeLevel <= 0.0150)
            {
                volumeSlider.value = 0.0200f;
                currentVolumeLevel = volumeSlider.value;
            }
            else
            {
                volumeSlider.value = currentVolumeLevel;
            }
        }
        else
        {
            currentVolumeLevel = volumeSlider.value;
            volumeSlider.value = volumeSlider.minValue;
            print(currentVolumeLevel);
        }
    }

    public void HandleSliderValueChanged(float volume)
    {
        audioMixer.SetFloat(groupName.ToString(), Mathf.Log10(volume) * modificationMultiplier);
        toggleButton.isOn = volumeSlider.value > volumeSlider.minValue;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(groupName.ToString(), volumeSlider.value);
    }


}