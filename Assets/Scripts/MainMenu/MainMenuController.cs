using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Game";
    public string storySceneName = "Story";

    // 0 = TopView, 1 = FPS
    const string StartModeKey = "StartMode";

    public void StartGame()
    {
        PlayerPrefs.SetInt(StartModeKey, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameSceneName);
    }

    public void Story()
    {
        PlayerPrefs.SetInt(StartModeKey, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(storySceneName); // ✅ 수정
    }

    public void Quit()
    {
        Application.Quit();
    }
}
