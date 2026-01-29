using UnityEngine;
using TMPro;

public class HUDTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    void OnEnable() // 씬에서 활성화 될 때 자동 실행, observer 구독
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.OnTimeChanged += UpdateUI;
            UpdateUI(GameFlowManager.Instance.ElapsedTime); // GameFlowManager에서 타이머를 통합으로 관리
        }
        else
        {
            Debug.LogError("[HUDTimerUI] GameFlowManager.Instance is null.");
        }
    }

    void OnDisable()
    {
        if (GameFlowManager.Instance != null)
            GameFlowManager.Instance.OnTimeChanged -= UpdateUI;
    }

    void UpdateUI(float elapsed) // Timer UI 업데이트
    {
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
