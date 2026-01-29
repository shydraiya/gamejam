using UnityEngine;

using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHP = 100f;
    [SerializeField] private float hp = 100f;

    [Header("Regen")]
    [SerializeField] private float hpRegenPerSecond = 0f;
    [SerializeField] private bool enableRegen = false;

    private Coroutine regenCoroutine;


    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 6f;
    [SerializeField] private float moveSpeedMultiplier = 1f;

    private DamageFlash damageFlash;
    [SerializeField] private bool invulnerable = false;

    [SerializeField] private bool isDead = false;

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

    private System.Collections.IEnumerator HpRegenRoutine()
    {
        var wait = new WaitForSeconds(1f); // timeScale 영향 받음
        while (true)
        {
            yield return wait;

            if (!enableRegen) continue;
            if (hp <= 0f) continue;        // 죽었으면 회복 X
            if (hp >= maxHP) continue;     // 풀피면 회복 X

            Heal(hpRegenPerSecond);
        }
    }

    public void AddHpRegen(float amount){
        if(!enableRegen) enableRegen = true;
        hpRegenPerSecond += amount;
    }


    void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        // 안전 초기화
        hp = Mathf.Clamp(hp, 0f, maxHP);
        OnHealthChanged?.Invoke(hp, maxHP);
        regenCoroutine = StartCoroutine(HpRegenRoutine());
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
        // 체력 색 변경
        float t = hp / maxHP;
        Color baseColor = Color.Lerp(Color.red, Color.green, t);
        damageFlash.SetBaseColor(baseColor);

        // 피격 이펙트 (반짝임)
        damageFlash.Flash();

        // hp가 0이 되면
        if (hp <= 0f && !isDead){
            isDead = true;
            OnDeath?.Invoke();
            GameFlowManager.Instance.TriggerGameOver();
        }
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
