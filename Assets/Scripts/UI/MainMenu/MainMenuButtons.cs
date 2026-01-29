using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{


    public void StartGame()
    {
        GameFlowManager.Instance.StartGame();
    }
    public void MainMenu()
    {
        GameFlowManager.Instance.GoToMainMenu();
    }

    public void Story()
    {
        GameFlowManager.Instance.GoToStory();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
