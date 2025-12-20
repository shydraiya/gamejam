using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MobEnemy : Enemy
{
    private Transform target;
    private Rigidbody rb;

    protected override void OnSpawned()
    {
        rb = GetComponent<Rigidbody>();

        // 물리 세팅(필요에 맞게)
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        // 탑다운이면 보통 Y축도 고정하지 않고 위치 이동만 하게 둬도 됩니다.

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - rb.position;
        if (toTarget.sqrMagnitude < 0.0001f) return;

        Vector3 direction = toTarget.normalized;
        Vector3 nextPos = rb.position + direction * Config.moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPos);
    }

    protected override void OnDied()
    {
        Debug.Log($"{name} died");
    }
}
