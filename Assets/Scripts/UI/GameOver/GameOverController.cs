using UnityEngine;

public class GameOverController : MonoBehaviour
{
    private PlayerStats player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerStats>();

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

        if (GameFlowManager.Instance == null)
        {
            Debug.LogError("[GameOverController] GameFlowManager.Instance is null. Ensure it exists (MainMenu scene bootstrap + DontDestroyOnLoad).");
            return;
        }

        GameFlowManager.Instance.TriggerGameOver();
    }
}
