using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    [SerializeField] private CameraModeController cameraMode;

    private DamageFlash damageFlash;
    PlayerProgression playerProgression;

    protected float Hp { get; private set; }
    protected EnemyConfig Config => config;

    protected virtual void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        Hp = config.maxHp;
        playerProgression = FindFirstObjectByType<PlayerProgression>();
        OnSpawned();
    }

    public void TakeDamage(float damage, bool isFPS)
    {

        // shotgun mode = Multiplier applied.
        float multiplier = (!isFPS) ? config.areaDamageMultiplier : 1;
        Hp -= damage * multiplier;
        Debug.Log($"[Enemy] Dealt {damage * multiplier}, fps:{isFPS}");
        damageFlash.Flash();
        if (Hp <= 0f) Die();
    }

    protected virtual void Die()
    {
        OnDied();
        if (playerProgression != null)
        {
            playerProgression.AddXP(config.xp_reward);
        }
        Destroy(gameObject);
    }

    public void InstantKill(){
        TakeDamage(1000000, false);
    }

    protected virtual void OnSpawned() { }
    protected virtual void OnDied() { }
}
