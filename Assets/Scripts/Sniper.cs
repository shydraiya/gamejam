using UnityEngine;

public class Sniper : MonoBehaviour, IWeapon
{

    [SerializeField] private CameraModeController cameraMode;

    [Header("Prefab")]
    public Bullet bulletPrefab;

    [Header("Fire")]
    public float fireInterval = 0.6f;
    public int pellets = 1;
    public float spreadAngle = 18f;      // 도 단위, Y축 기준 퍼짐
    public float bulletSpeed = 100f;
    public float damagePerPellet = 200f;

    [Header("Spawn")]
    public float spawnHeight = 0.6f;     // 바닥 박힘 방지
    public float spawnForwardOffset = 0.5f;

    [Header("Hit")]
    public LayerMask hitMask = ~0;       // Enemy 레이어만 추천

    PlayerWeaponController owner;
    bool active = true;
    float nextFireTime = 0f;

    public void SetOwner(PlayerWeaponController owner) => this.owner = owner;
    public void SetActive(bool active) => this.active = active;

    public void Fire()
    {
        if (!active || owner == null || bulletPrefab == null) return;
        if (Time.time < nextFireTime) return;

        // 탑뷰 샷건 전용: aimDir을 XZ 평면으로 투영
        Vector3 baseDir = owner.GetFPSAimDirection();
        if (baseDir.sqrMagnitude < 0.0001f) return;
        baseDir.Normalize();

        Vector3 origin = owner.transform.position + Vector3.up * spawnHeight + baseDir * spawnForwardOffset;

        for (int i = 0; i < pellets; i++)
        {
            float t = (pellets == 1) ? 0.5f : (i / (pellets - 1f));
            float angle = Mathf.Lerp(-spreadAngle * 0.5f, spreadAngle * 0.5f, t);

            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;
            dir.Normalize();

            // Bullet 생성 + 초기화
            Bullet b = Instantiate(bulletPrefab, origin, Quaternion.LookRotation(dir, Vector3.up));
            b.Init(dir * bulletSpeed, damagePerPellet, hitMask, cameraMode);
        }

        nextFireTime = Time.time + fireInterval;
    }
}
