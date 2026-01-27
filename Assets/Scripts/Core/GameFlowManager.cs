using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
/// <summary>
/// 게임 상태, 타이머, 게임오버 처리, 재시작 처리를 담당.
/// UI는 표시만
/// </summary>
public enum GameState
{
    Running,
    Paused,
    GameOver
}

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameScene = "Game";
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string storyScene = "Story";

    [Header("Time")]
    [SerializeField] private bool useUnscaledTime = true;

    public GameState State { get; private set; } = GameState.Running;

    public float ElapsedTime { get; private set; } = 0f;

    // observers
    // 외부에서 구독해서 사용
    public event Action<GameState> OnStateChanged; //게임 상태 변경시
    public event Action<float> OnTimeChanged; // elapsed seconds
    public event Action<float> OnGameOver;    // final time

    void Awake()
    {
        // singleton : 중복체크
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 모든 씬에서 이거 하나로 씬 이동함

        State = GameState.Running;
        ElapsedTime = 0f;
    }

    // 시간 update 관리
    void Update()
    {
        if (SceneManager.GetActiveScene().name != gameScene) return;
        if (State != GameState.Running) return;
        // 시간 update
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        ElapsedTime += dt;
        OnTimeChanged?.Invoke(ElapsedTime);
    }

    /// <summary>
    /// Game 씬 불러오기.
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1f;        // 시간 배율 x1 
        State = GameState.Running;
        ElapsedTime = 0f;           // 타이머 초기화
        OnStateChanged?.Invoke(State);
        OnTimeChanged?.Invoke(ElapsedTime);

        SceneManager.LoadScene(gameScene);
    }

    // 게임 state 확인 (paused or running)
    // GameOver 상태에선 작동 x
    public void Pause(bool pause)
    {
        if (State == GameState.GameOver) return;

        State = pause ? GameState.Paused : GameState.Running;
        Time.timeScale = pause ? 0f : 1f;
        OnStateChanged?.Invoke(State);
    }

    /// <summary>
    /// state를 GameOver로 바꾸고
    /// listener 들에게 전파
    /// </summary>
    public void TriggerGameOver()
    {
        if (State == GameState.GameOver) return;

        State = GameState.GameOver;
        OnStateChanged?.Invoke(State);
        OnGameOver?.Invoke(ElapsedTime);
        
        Time.timeScale = 1f;
        StartCoroutine(LoadGameOverAfterDelay());
        
    }

    private IEnumerator LoadGameOverAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(gameOverScene);
    }

    public void Retry()
    {
        StartGame();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GoToStory()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(storyScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
