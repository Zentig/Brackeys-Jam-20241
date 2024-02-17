using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider soundMasterSlider;
    public Slider mouseSensitivitySlider;

    private void Start()
    {
        soundMasterSlider.value = PlayerPrefs.GetFloat("SoundMasterVolume", 1.0f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    }

    public void SetSoundMasterVolume(float volume)
    {
        // Camera.main.GetComponent<AudioListener>()
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("SoundMasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerPrefs.Save();
    }
}
