using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelLoader.LoadNextLevel(0.2f, 1);
        }
    }
}
