using UnityEngine;

using TMPro; // TextMeshPro 사용 시

public class EndingTimeText : MonoBehaviour
{
    private void Start()
    {
        float time = GameTimeTracker.FinalTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        string formatted = $"{minutes:00}:{seconds:00}";

        // TextMeshPro
        var tmp = GetComponent<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = $"Your Final Time\n{formatted}";
            return;
        }

        // Legacy UI Text
        var text = GetComponent<UnityEngine.UI.Text>();
        if (text != null)
        {
            text.text = $"Your Final Time\n{formatted}";
        }
    }
}
