using UnityEngine;

using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;

    protected float Hp { get; private set; }
    protected EnemyConfig Config => config;

    protected virtual void Awake()
    {
        Hp = config.maxHp;
        OnSpawned();
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
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
