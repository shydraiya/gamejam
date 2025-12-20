using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
    public float spawnIntervalMob = 3f;
    public float spawnIntervalBoss = 4f;
    public float spawnRadius = 10f;
}
