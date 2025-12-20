using UnityEngine;

public class MobEnemy : Enemy
{
    private Transform target;

    protected override void OnSpawned()
    {
        // 플레이어 찾기 (태그 사용 권장)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Inflict()
    {
        this.TakeDamage(1f);
    }

    private void OnMouseOver()
    {
        Inflict();
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * Config.moveSpeed * Time.deltaTime;
    }

    protected override void OnDied()
    {
        Debug.Log($"{name} died");
    }

}