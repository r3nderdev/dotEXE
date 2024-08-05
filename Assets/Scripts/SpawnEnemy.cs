using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private int indexToSpawnOn;
    [SerializeField] private Dialogue dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitUntil(() => dialogue.index == indexToSpawnOn);
        spawn.SetActive(true);
    }
}
