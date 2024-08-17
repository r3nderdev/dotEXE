using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class SettingsMenu : MonoBehaviour

{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Pixelation Effect")]
    [SerializeField] private Pixelate pixelEffect;
    [SerializeField] private Image pixelToggleImage;

    [Header("ASCII Effect")]
    [SerializeField] private ASCIIRendering asciiEffect;
    [SerializeField] private Image asciiToggleImage;



    [Header("Settings Sliders")]
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
            pixelToggleImage.color = Color.white;

        }
        if (!pixelation)
        {
            // Turn pixel off
            pixelEffect.enabled = false;
            pixelToggleImage.color = Color.black;
        }
    }

    public void SetASCII(bool ascii)
    {
        // Toggling ASCII Effect
        if (ascii)
        {
            // Turn ascii effect on
            asciiEffect.enabled = true;
            asciiToggleImage.color = Color.white;

        }
        if (!ascii)
        {
            // Turn ascii off
            asciiEffect.enabled = false;
            asciiToggleImage.color = Color.black;
        }
    }
}
