using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Game";

    // 0=TopView, 1=FPS
    const string StartModeKey = "StartMode";

    public void StartGame()
    {
        PlayerPrefs.SetInt(StartModeKey, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
