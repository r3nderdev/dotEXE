using System.Collections;
using UnityEngine;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject welcomeText;
    [SerializeField] private GameObject inputFieldObject;
    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject passwordErrorText;
    [SerializeField] private GameObject loadingIcon;

    [Header("dotEXE")]
    [SerializeField] private DotEXELogin DotEXELogin;

    private TMP_InputField inputField;
    private LoadingSpin spin;

    private void Start()
    {
        spin = loadingIcon.GetComponent<LoadingSpin>();
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.PlaySound(SoundType.PC_BOOT, 1f);
    }



    public void Login()
    {
        // If something is written in the field
        if (inputField.text != "")
        {
            passwordErrorText.SetActive(false);

            inputFieldObject.SetActive(false);
            loginButton.SetActive(false);
            loadingIcon.SetActive(true);

            StartCoroutine(HidePauseScreen());
        }

        else
        {
            // show password error
            passwordErrorText.SetActive(true);
        }
    }

    IEnumerator HidePauseScreen()
    {
        // holy shit
        yield return new WaitForSeconds(2);

        // glitch loading icon
        spin.glitched = true;
        SoundManager.PlaySound(SoundType.PC_GLITCH, 0.4f);

        yield return new WaitForSeconds(1.7f);

        SoundManager.PlaySound(SoundType.PC_GLITCH, 0.4f);

        yield return new WaitForSeconds(1);

        loadingIcon.SetActive(false);

        yield return new WaitForSeconds(.7f);

        SoundManager.PlaySound(SoundType.PC_GLITCH, 0.4f);

        inputFieldObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        passwordErrorText.SetActive(true);
        inputFieldObject.SetActive(false);
        welcomeText.SetActive(false);


        yield return new WaitForSeconds(0.5f);

        welcomeText.SetActive(true);
        passwordErrorText.SetActive(false);

        yield return new WaitForSeconds(0.6f);

        welcomeText.SetActive(false);
        background.SetActive(false);

        SoundManager.PlaySound(SoundType.PC_SHUTDOWN, 1f);

        yield return new WaitForSeconds(3);

        DotEXELogin.ShowScreen();
    }
}
