using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotEXELogin : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] GameObject welcomeText;
    [SerializeField] GameObject chanceText;
    [SerializeField] GameObject joinText;
    [SerializeField] GameObject joinButton;

    [Header("Level Loader")]
    [SerializeField] private LevelLoader levelLoader;

    public void ShowScreen()
    {
        background.SetActive(true);
        StartCoroutine(ShowScreenRoutine());
    }

    IEnumerator ShowScreenRoutine()
    {
        SoundManager.PlaySound(SoundType.PC_BOOT, 1f);
        yield return new WaitForSeconds(3);
        welcomeText.SetActive(true);
        SoundManager.PlaySound(SoundType.PC_DARK, 1f);
        yield return new WaitForSeconds(4);
        chanceText.SetActive(true);
        yield return new WaitForSeconds(3);
        joinText.SetActive(true);
        yield return new WaitForSeconds(1);
        joinButton.SetActive(true);
    }

    public void Join()
    {
        SoundManager.PlaySound(SoundType.PC_LOGIN, 1f);
        joinButton.SetActive(false);
        StartCoroutine(JoinCoRoutine());
    }

    IEnumerator JoinCoRoutine()
    {
        yield return new WaitForSeconds(2f);
        welcomeText.SetActive(false);
        yield return new WaitForSeconds(1f);
        chanceText.SetActive(false);
        yield return new WaitForSeconds(1f);
        joinText.SetActive(false);
        yield return new WaitForSeconds(1f);
        background.SetActive(false);
        levelLoader.LoadNextLevel(0.5f, 1);
    }
}
