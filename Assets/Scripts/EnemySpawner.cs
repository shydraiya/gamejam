using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float SPAWN_INTERVAL = 2f;
    private const float SPAWN_RADIUS = 5f;

    [SerializeField] private Enemy enemyPrefab;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, SPAWN_INTERVAL);
    }

    private void Spawn()
    {
        Vector2 randomOffset = Random.insideUnitCircle * SPAWN_RADIUS;
        Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
