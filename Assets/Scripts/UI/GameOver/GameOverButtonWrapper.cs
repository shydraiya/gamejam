using UnityEngine;

public class GameOverButtonWrapper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void retry()
    {
        GameFlowManager.Instance.Retry();
    }

    public void ToMain()
    {
        GameFlowManager.Instance.GoToMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
