using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool useUnscaledTime = false;

    private float elapsed;

    public float ElapsedTime => elapsed;

    void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        elapsed += dt;

        UpdateUI();
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        elapsed = 0f;
        UpdateUI();
    }
}
