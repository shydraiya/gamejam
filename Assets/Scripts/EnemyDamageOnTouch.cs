using UnityEngine;

public class EnemyDamageOnTouch : MonoBehaviour
{
    public float hitInterval = 0.5f; // 연속 접촉 데미지 간격
    public EnemyConfig config;
    float nextHitTime = 0f;

    void OnTriggerStay(Collider other)
    {
        if (Time.time < nextHitTime) return;

        PlayerStats player = other.GetComponentInParent<PlayerStats>();
        if (player == null) return;

        player.TakeDamage(config.mob_damage);
        nextHitTime = Time.time + hitInterval;
    }
}
