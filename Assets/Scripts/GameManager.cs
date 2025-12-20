using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerProgression progression;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private LevelUpUI levelUpUI;

    private void Start()
    {
        // 필수 참조 확인
        Debug.Assert(progression != null, "PlayerProgression not assigned");
        Debug.Assert(upgradeManager != null, "UpgradeManager not assigned");
        Debug.Assert(levelUpUI != null, "LevelUpUI not assigned");

        levelUpUI.upgradeManager = upgradeManager;
        progression.OnLevelUp += HandleLevelUp;

        levelUpUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (progression != null)
            progression.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp()
    {
        // Time.timeScale = 0f;
        levelUpUI.Show();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
