using TMPro;

using UnityEngine;

public class HPUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private PlayerStats playerStats; // 또는 PlayerHealth

    void Awake()
    {
        if (hpText == null) hpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (playerStats == null) return;

        float hp = playerStats.HP;
        float maxHP = playerStats.MaxHP;

        hpText.text = $"HP: {Mathf.CeilToInt(hp)}/{Mathf.CeilToInt(maxHP)}";
    }
}
