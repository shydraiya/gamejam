using UnityEngine;

public class MobBoss : Enemy
{
    private Transform target;

    protected override void OnSpawned()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        target = player != null ? player.transform : null;
    }

    private void Update()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * Config.moveSpeed * Time.deltaTime;
    }

    protected override void OnDied()
    {
        Debug.Log($"{name} died");
    }
}
