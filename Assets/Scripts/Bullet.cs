using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float lifeTime = 2.0f;
    public LayerMask hitMask = ~0;

    float damage;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Init(Vector3 velocity, float damage, LayerMask hitMask)
    {
        this.damage = damage;
        this.hitMask = hitMask;

        rb.linearVelocity = velocity;

        CancelInvoke(nameof(Die));
        Invoke(nameof(Die), lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        ApplyDamageIfValid(collision.collider);
        // Die(); // 어떤 충돌이든 무조건 사라짐
    }

    void OnTriggerEnter(Collider other)
    {
        ApplyDamageIfValid(other);
        Die(); // 트리거여도 무조건 사라짐
    }

    void ApplyDamageIfValid(Collider col)
    {
        int layerBit = 1 << col.gameObject.layer;
        if ((hitMask.value & layerBit) == 0)
            return;

        Enemy enemy = col.GetComponentInParent<Enemy>();
        if (enemy != null){
            enemy.TakeDamage(damage);
            Debug.Log($"bullet hit");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
