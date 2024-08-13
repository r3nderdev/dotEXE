using UnityEngine;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TMP_Text[] buttonTexts;
    [SerializeField] private LevelLoader levelLoader;


    [Header("Values")]
    [SerializeField] private float levelloadDelay;
    private void Start()
    {

        foreach (TMP_Text text in buttonTexts)
        {
            Color textColor = text.color;
            textColor.a = 1; // Fully visible
            text.color = textColor;
        }
    }


    public void PlayGame()
    {
        cameraAnimator.SetTrigger("StartAnimation");

        StartCoroutine(FadeOut());
    }


    // UI Fade
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.7f)
        {
            elapsedTime += Time.deltaTime;

            foreach (TMP_Text text in buttonTexts)
            {
                Color textColor = text.color;
                textColor.a = Mathf.Lerp(1, 0, elapsedTime / 0.7f);
                text.color = textColor;
            }

            yield return null;
        }

        levelLoader.LoadNextLevel(levelloadDelay, 1);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
