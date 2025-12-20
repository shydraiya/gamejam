using UnityEngine;

public class TopViewShotgun : MonoBehaviour, IWeapon
{
    [Header("Tuning")]
    public float fireInterval = 0.8f;
    public int pellets = 6;
    public float spreadAngle = 18f;
    public float range = 12f;
    public float damagePerPellet = 12f;

    [Header("Targeting")]
    public float searchRadius = 15f;
    public LayerMask enemyMask;

    bool active = true;
    float timer = 0f;

    public void SetActive(bool a)
    {
        active = a;
        timer = 0f; // 켤 때 즉시 발사 원하면 0, 지연 원하면 fireInterval로
    }

    // 컨트롤러에서 "지금 당장 한 번 발사"를 시키고 싶으면 호출
    public void Fire()
    {
        ShotgunFireOnce();
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ShotgunFireOnce();
            timer = fireInterval;
        }
    }

    void ShotgunFireOnce()
    {
        Vector3 origin = transform.position + Vector3.up * 0.6f;
        Vector3 baseDir = GetAutoAimDirection(origin);

        for (int i = 0; i < pellets; i++)
        {
            float t = (pellets == 1) ? 0f : (i / (pellets - 1f));
            float angle = Mathf.Lerp(-spreadAngle * 0.5f, spreadAngle * 0.5f, t);
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;

            Debug.DrawRay(origin, dir * range, Color.cyan, 0.1f);

            if (Physics.Raycast(origin, dir, out RaycastHit hit, range, enemyMask, QueryTriggerInteraction.Ignore))
            {
                var health = hit.collider.GetComponentInParent<Health>();
                if (health != null) health.TakeDamage(damagePerPellet);
            }
        }
    }

    Vector3 GetAutoAimDirection(Vector3 origin)
    {
        Collider[] hits = Physics.OverlapSphere(origin, searchRadius, enemyMask, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0)
            return Vector3.forward;

        Transform nearest = null;
        float best = float.MaxValue;

        foreach (var c in hits)
        {
            float d = (c.transform.position - origin).sqrMagnitude;
            if (d < best)
            {
                best = d;
                nearest = c.transform;
            }
        }

        Vector3 dir = (nearest.position - origin);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return Vector3.forward;
        return dir.normalized;
    }
}
