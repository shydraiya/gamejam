using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
  public void OnClickRetry()
    {
        GameFlowManager.Instance.Retry();
    }

    public void OnClickMainMenu()
    {
        GameFlowManager.Instance.GoToMainMenu();
    }

    public void OnClickQuit()
    {
        GameFlowManager.Instance.Quit();
    }
}
