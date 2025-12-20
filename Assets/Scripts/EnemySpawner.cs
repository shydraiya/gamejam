using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Enemy bossPrefab;

    [SerializeField] private SpawnerConfig config;

    protected float Hp { get; private set; }
    protected SpawnerConfig Config => config;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMob), 0f, config.spawnIntervalMob);
        InvokeRepeating(nameof(SpawnBoss), 0f, config.spawnIntervalBoss);
    }

    private void SpawnMob()
    {
        for (int i = 0; i < config.spawnIntervalMobCnt; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * config.spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void SpawnBoss()
    {
        for (int i = 0; i < config.spawnIntervalBossCnt; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * config.spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        }
    }
}
