using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFader : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Fade Settings")]
    [Range(0, 1)]
    [SerializeField] private float startVolume = 0f;
    [Range(0, 1)]
    [SerializeField] private float endVolume = 1f;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        // Set starting volume
        if (audioSource != null)
        {
            audioSource.volume = startVolume;
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned in the AudioFader script.");
        }
    }

    private void Start()
    {
        StartCoroutine(FadeAudio());
    }

    private IEnumerator FadeAudio()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Calculate the new volume
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = endVolume;
    }
}