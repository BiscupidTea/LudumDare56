using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider SfxSlider;
    [SerializeField] private Slider MusicSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            MasterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            SfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        SetMasterVolume();
        SetSFXVolume();
        SetMusicVolume();
    }

    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = MusicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
}