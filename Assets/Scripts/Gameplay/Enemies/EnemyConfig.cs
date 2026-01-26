using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public const float MAX_HP = 100f;
    public const float MOVE_SPEED = 3f;
    public float maxHp = MAX_HP;
    public float moveSpeed = MOVE_SPEED;

    public float xp_reward = 1;

    public float mob_damage = 1;

    // 뱀서/플랫포머 밸런스용
    public float areaDamageMultiplier = 1f; // 보스는 0.1 같은 값
}
