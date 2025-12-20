using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameOverSceneName = "GameOver";

    [Header("Timer Source (optional)")]
    [SerializeField] private GameTimer gameTimer;          // 있으면 사용
    [SerializeField] private GameTimeTracker timeTracker;  // 있으면 사용

    private PlayerStats player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerStats>();

        // 인스펙터에 안 넣었으면 자동으로 찾기
        if (!gameTimer) gameTimer = FindFirstObjectByType<GameTimer>();
        if (!timeTracker) timeTracker = FindFirstObjectByType<GameTimeTracker>();

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

        // timeScale=0 이면 코루틴 대기/씬 전환이 멈출 수 있으니 원복
        Time.timeScale = 1f;

        // 1) 생존 시간 저장: GameTimeTracker가 있으면 그걸 우선 사용
        if (timeTracker != null)
        {
            timeTracker.SaveFinalTime(); // 내부에서 PlayerPrefs 저장하도록 되어있다면 그대로 사용
        }
        else
        {
            // 2) 없으면 여기서 PlayerPrefs로 직접 저장
            float survivedSec = (gameTimer != null) ? gameTimer.ElapsedTime : Time.time;
            PlayerPrefs.SetFloat("SurvivedTimeSec", survivedSec);
            PlayerPrefs.Save();
        }

        StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadGameOver()
    {
        // timeScale 영향 없이 1초 대기
        yield return new WaitForSecondsRealtime(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(gameOverSceneName);
    }
}
