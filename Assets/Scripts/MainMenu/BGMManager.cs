using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioSource audioSource;

    [Header("MainMenu / Game BGM (랜덤 1곡)")]
    public AudioClip[] menuAndGameBGMs; // 3곡

    [Header("GameOver BGM")]
    public AudioClip gameOverBGM;

    [Header("Credit BGM")]
    public AudioClip creditBGM;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
            case "Game":
                PlayRandomMenuGameBGM();
                break;

            case "GameOver":
                PlayBGM(gameOverBGM);
                break;
            case "Story":
                PlayBGM(creditBGM);
                break;
        }
    }

    void PlayRandomMenuGameBGM()
    {
        if (menuAndGameBGMs == null || menuAndGameBGMs.Length == 0)
            return;

        AudioClip selected;
        do
        {
            selected = menuAndGameBGMs[Random.Range(0, menuAndGameBGMs.Length)];
        }
        while (menuAndGameBGMs.Length > 1 && selected == audioSource.clip);


        PlayBGM(selected);
    }

    void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        StopAllCoroutines();
        StartCoroutine(FadeAndPlay(clip));
    }

    IEnumerator FadeAndPlay(AudioClip newClip)
    {
        // Fade Out
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade In
        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime;
            yield return null;
        }
    }
}
