using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Mixer")]
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

    private float volumeValue;

    private void Awake()
    {
        if (pixelEffect != null)
        {
            if (PlayerPrefs.HasKey("Pixelation")) SetPixelation(bool.Parse(PlayerPrefs.GetString("Pixelation")));
        }

        if (asciiEffect != null)
        {
            if (PlayerPrefs.HasKey("ASCII")) SetASCII(bool.Parse(PlayerPrefs.GetString("ASCII")));
        }
    }

    private void Start()
    {
        float sens = PlayerLook.sensitivity;
        sensitivitySlider.value = sens;

        if (audioMixer.GetFloat("Volume", out volumeValue))
        {
            volumeSlider.value = volumeValue;
        }
    }

    public void SetSensitivity(float sens)
    {
        PlayerLook.sensitivity = sens;
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

            PlayerPrefs.SetString("Pixelation", "True");
        }
        if (!pixelation)
        {
            // Turn pixel off
            pixelEffect.enabled = false;
            pixelToggleImage.color = Color.black;

            PlayerPrefs.SetString("Pixelation", "False");
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

            PlayerPrefs.SetString("ASCII", "True");
        }
        if (!ascii)
        {
            // Turn ascii off
            asciiEffect.enabled = false;
            asciiToggleImage.color = Color.black;

            PlayerPrefs.SetString("ASCII", "False");
        }
    }
}
