using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    public void LoadNextLevel(float delay, int buildIndexAdd)
    {
        // Load next scene
        StartCoroutine(LoadLevel((SceneManager.GetActiveScene().buildIndex + buildIndexAdd), delay));
    }

    IEnumerator LoadLevel (int levelIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        transition.SetTrigger("Start");

        PlayerHealth.killCount = 0;

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
