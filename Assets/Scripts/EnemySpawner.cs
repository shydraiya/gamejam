using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Enemy bossPrefab;

    [SerializeField] private SpawnerConfig config;

    protected float Hp { get; private set; }
    protected SpawnerConfig Config => config;

    private void Start()
    {
        StartCoroutine(SpawnMobRoutine());
        StartCoroutine(SpawnBossRoutine());
        // InvokeRepeating(nameof(SpawnMob), 0f, config.spawnIntervalMob);
        // InvokeRepeating(nameof(SpawnBoss), 0f, config.spawnIntervalBoss);
    }

    IEnumerator SpawnMobRoutine()
    {
        float elapsed = 0f;

        while (true)
        {
            float interval = Mathf.Max(
                0.25f,
                config.spawnIntervalMob / (1f + elapsed * 0.05f)
            );

            SpawnMob();
            yield return new WaitForSeconds(interval);

            elapsed += interval;
        }
    }


    IEnumerator SpawnBossRoutine()
    {
        float elapsed = 0f;
    
        while (true)
        {
            float interval = Mathf.Max(
                0.25f,
                config.spawnIntervalBoss / (1f + elapsed * 0.05f)
            );
    
            SpawnBoss();
            yield return new WaitForSeconds(interval);
    
            elapsed += interval;
        }
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
