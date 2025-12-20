using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
    public float spawnIntervalMob = 3f;
    public int spawnIntervalMobCnt = 10;
    public float spawnIntervalBoss = 10f;
    public int spawnIntervalBossCnt = 1;
    public float spawnRadius = 10f;
}
