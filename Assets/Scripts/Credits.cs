using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;

        levelLoader.LoadNextLevel(0f, -6);
    }
}
