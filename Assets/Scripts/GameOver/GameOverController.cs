using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameOverSceneName = "GameOver";

    private PlayerStats player;

    private GameTimeTracker timeTracker;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerStats>();

        timeTracker = FindFirstObjectByType<GameTimeTracker>();
        if (player != null)
        {
            player.OnDeath += OnPlayerDeath;
        }
        else
        {
            Debug.LogError("[GameOverController] PlayerStats not found.");
        }
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("[GameOverController] OnPlayerDeath invoked");

        if (timeTracker != null)
            timeTracker.SaveFinalTime();
        StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }
}
