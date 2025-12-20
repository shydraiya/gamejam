using UnityEngine;

public class BossEnemy : Enemy
{
    protected override void OnSpawned()
    {
        // 스폰 시 필요한 초기화
        // 예: 이펙트, 이동 시작 등
        Debug.Log($"{name} (boss) spawned with HP");
    }

    protected override void OnDied()
    {
        // 사망 시 처리
        // 예: 점수, 드랍, 이펙트
        Debug.Log($"{name} (boss) died");
    }

}