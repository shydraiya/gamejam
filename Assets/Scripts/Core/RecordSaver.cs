using UnityEngine;
public class RecordSaver : MonoBehaviour
{
    void OnEnable()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.OnGameOver += Save;
    }

    void OnDisable()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.OnGameOver -= Save;
    }

    void Save(float finalTime)
    {
        PlayerPrefs.SetFloat("SurvivedTimeSec", finalTime);
        PlayerPrefs.Save();
    }
}
