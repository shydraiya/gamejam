using UnityEngine;

using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float hp = 100f;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 6f;
    [SerializeField] public float moveSpeedMultiplier = 1f;

    [SerializeField] private bool invulnerable = false;
    public bool Invulnerable => invulnerable;

    public void SetInvulnerable(bool value)
    {
        invulnerable = value;
    }


    public float MaxHP => maxHP;
    public float HP => hp;

    // 최종 이동속도(컨트롤러는 이 값을 사용)
    public float MoveSpeed => baseMoveSpeed * moveSpeedMultiplier;

    public event Action<float, float> OnHealthChanged; // (hp, maxHP)
    public event Action OnDeath;

    void Awake()
    {
        // 안전 초기화
        hp = Mathf.Clamp(hp, 0f, maxHP);
        OnHealthChanged?.Invoke(hp, maxHP);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        float prev = hp;
        hp = Mathf.Min(maxHP, hp + amount);
        if (!Mathf.Approximately(prev, hp))
            OnHealthChanged?.Invoke(hp, maxHP);
    }

    public void TakeDamage(float amount)
    {
        if (invulnerable) return;
        if (amount <= 0f) return;
        float prev = hp;
        hp = Mathf.Max(0f, hp - amount);
        if (!Mathf.Approximately(prev, hp))
            OnHealthChanged?.Invoke(hp, maxHP);

        if (hp <= 0f)
            OnDeath?.Invoke();
    }

    // 업그레이드에서 쓰기 좋은 API
    public void AddMoveSpeedMultiplier(float addPercent)
    {
        // addPercent: 0.10f => +10%
        moveSpeedMultiplier *= (1f + addPercent);
    }

    public void AddMaxHP(float amount, bool healToFill = true)
    {
        if (amount <= 0f) return;
        maxHP += amount;
        if (healToFill) hp = maxHP;
        else hp = Mathf.Min(hp, maxHP);
        OnHealthChanged?.Invoke(hp, maxHP);
    }

}
