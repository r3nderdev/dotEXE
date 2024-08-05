using System.Collections;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioStart;
    [SerializeField] private AudioSource audioLoop; 

    private void Start()
    {
        audioStart.Play();

        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitUntil(()=> audioStart.isPlaying == false);
        audioLoop.Play();
    }
}
