using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Button[] menuButtons;
    private TMP_Text[] buttonTexts;

    [SerializeField] private GameObject computerUI;

    [Header("Delay")]
    [SerializeField] private float animationDelay = 4f;

    private void Start()
    {
        buttonTexts = new TMP_Text[3];
        int index = 0;

        // Populate the buttonTexts array
        foreach (Button button in menuButtons)
        {
            TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                buttonTexts[index] = text;
                index++;
            }
        }

        // Set text color to fully visible
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

    private IEnumerator FadeOut()
    {
        foreach (Button button in menuButtons)
        {
            button.interactable = false;
        }

        // Fade UI
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

        yield return new WaitForSeconds(animationDelay);

        foreach (Button button in menuButtons)
        {
            button.transform.parent.gameObject.SetActive(false);
        }

        // Activate Computer UI
        computerUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}