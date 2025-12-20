using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public float maxHp = 10f;
    public float moveSpeed = 3f;

    // 뱀서/플랫포머 밸런스용
    public float areaDamageMultiplier = 1f; // 보스는 0.1 같은 값
}
