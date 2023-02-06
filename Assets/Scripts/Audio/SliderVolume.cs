using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System;

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
    [SerializeField] private PlayerControllerSO playerController;

    [Header("Preview AudioSource")]
    [SerializeField] public AudioSource previewSound;
    private float lastSliderValue;
    [SerializeField] private bool debugSound = true;

    private bool enableSound = false;
    private bool actuallySelected = false;

    private float currentVolumeLevel;
    private float modificationMultiplier = 30f;


    private void Awake()
    {
        volumeSlider = this.transform.GetComponentInChildren<Slider>();
        audioMixer.GetFloat(groupName.ToString(), out float volume);
        volumeSlider.value = volume + 1;
        currentVolumeLevel = volumeSlider.value;
    }

    private void Start()
    {
        playerController.GetPlayerController().playerInput.Audio.MuteSource.performed += MuteSourceVolume;
    }

    private void MuteSourceVolume(InputAction.CallbackContext ctx)
    {
        if (actuallySelected)
            HandleToggleValueChanged();
    }

    public void HandleToggleValueChanged()
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
            enableSound = false;
        }
        else
        {
            currentVolumeLevel = volumeSlider.value;
            volumeSlider.value = volumeSlider.minValue;
            enableSound = true;
        }
    }

    public void HandleSliderValueChanged(float volume)
    {
        audioMixer.SetFloat(groupName.ToString(), Mathf.Log10(volume) * modificationMultiplier);
        toggleButton.isOn = volumeSlider.value > volumeSlider.minValue;

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

    public void SetSeleted(bool selected)
    {
        if(selected)
            actuallySelected = true;
        else
            actuallySelected = false;
    }

    private void OnEnable()
    {
        playerController.GetPlayerController().playerInput.Audio.MuteSource.Enable();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(groupName.ToString(), volumeSlider.value);
        playerController.GetPlayerController().playerInput.Audio.MuteSource.Disable();
    }
}
