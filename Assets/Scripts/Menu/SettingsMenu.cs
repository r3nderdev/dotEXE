using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour

{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Pixelation Effect")]
    [SerializeField] private Pixelate pixelEffect;
    [SerializeField] private Image toggleBackground;
    [SerializeField] private Color white;
    [SerializeField] private Color black;

    [Header("Game Sliders")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    

    private void Start()
    {
        float sens = PlayerLook.mouseSensitivity;

        float volumeValue;

        if (audioMixer.GetFloat("Volume", out volumeValue))
        {
            volumeSlider.value = volumeValue;
        }

        sensitivitySlider.value = sens;

        white.a = 1f;
        black.a = 1f;
    }

    public void SetSensitivity(float sens)
    {
        PlayerLook.mouseSensitivity = sens;
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetPixelation (bool pixelation)
    {
        // Toggling Pixelation Effect
        if (pixelation)
        {
            // Turn pixel on
            pixelEffect.enabled = true;
            toggleBackground.color = white;

        }
        if (!pixelation)
        {
            // Turn pixel off
            pixelEffect.enabled = false;
            toggleBackground.color = black;
        }
    }
}
