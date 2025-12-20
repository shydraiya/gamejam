using UnityEngine;
using TMPro;

public class GameOverTimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    void Start()
    {
        float sec = PlayerPrefs.GetFloat("SurvivedTimeSec", 0f);
        int m = Mathf.FloorToInt(sec / 60f);
        int s = Mathf.FloorToInt(sec % 60f);

        if (timeText) timeText.text = $"{m:00}:{s:00}";
    }
}
