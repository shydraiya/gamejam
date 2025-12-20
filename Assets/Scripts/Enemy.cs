using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    [SerializeField] private CameraModeController cameraMode;

    protected float Hp { get; private set; }
    protected EnemyConfig Config => config;

    protected virtual void Awake()
    {
        Hp = config.maxHp;
        OnSpawned();
    }

    public void TakeDamage(float damage, bool isFPS)
    {

        // shotgun mode = Multiplier applied.
        float multiplier = (!isFPS) ? config.areaDamageMultiplier : 1;
        Hp -= damage * multiplier;
        Debug.Log($"[Enemy] Dealt {damage * multiplier}, fps:{isFPS}");
        if (Hp <= 0f) Die();
    }

    protected virtual void Die()
    {
        OnDied();
        Destroy(gameObject);
    }

    protected virtual void OnSpawned() { }
    protected virtual void OnDied() { }
}
