using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.EventSystems;

public class SliderVolume : MonoBehaviour
{

    enum MixerGroup
    {
        Music,
        SFX,
        Environment
    };

    [Header("Ref")]
    public AudioMixer audioMixer;
    private Slider volumeSlider;
    [SerializeField] private MixerGroup groupName;
    [SerializeField] private Toggle toggleButton;

    [Header("Preview AudioSource")]
    [SerializeField] public AudioSource previewSound;
    private float lastSliderValue;
    [SerializeField] private bool debugSound = true;

    private float currentVolumeLevel;
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
            if (currentVolumeLevel <= 0.0150)
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
        }
    }

    public void HandleSliderValueChanged(float volume)
    {
        audioMixer.SetFloat(groupName.ToString(), Mathf.Log10(volume) * modificationMultiplier);
        toggleButton.isOn = volumeSlider.value > volumeSlider.minValue;

        //if (debugSound)
        //{
        //    if (volumeSlider.value != lastSliderValue)
        //    {
        //        if (!previewSound.isPlaying && debugSound)
        //        {
        //            previewSound.volume = volumeSlider.value;
        //            previewSound.Play();
        //        }
        //    }
        //    else if (volumeSlider.value == lastSliderValue)
        //    {
        //        print(previewSound + " is stopped");
        //        previewSound.Stop();
        //    }
        //    lastSliderValue = volumeSlider.value;
        //}
    }
    public void PlayPreviewAudio()
    {
        if(debugSound)
            previewSound.Play();
    }
    public void StopPreviewAudio()
    {
        if (previewSound.isPlaying)
            previewSound.Stop();
    }



    private void OnDisable()
    {
        PlayerPrefs.SetFloat(groupName.ToString(), volumeSlider.value);
    }
}
