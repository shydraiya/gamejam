using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public Button[] buttons;
    public Text[] texts;

    private CursorLockMode prevCursorState;
    private bool prevCursorVisibility;
    List<UpgradeOption> current;

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        prevCursorState = Cursor.lockState;
        prevCursorVisibility = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        current = upgradeManager.GetRandomOptions(3);

        for (int i = 0; i < buttons.Length; i++)
        {
            int idx = i;
            texts[i].text = current[i].title;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() =>
            {
                upgradeManager.ApplyUpgrade(current[idx].type);
                Hide();
            });
        }
    }

    void Hide()
    {
        Time.timeScale = 1f;
        Cursor.lockState = prevCursorState;
        Cursor.visible = prevCursorVisibility;
        gameObject.SetActive(false);
    }
}
