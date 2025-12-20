using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHP = 100f;
    float hp;

    void Awake() => hp = maxHP;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        Debug.Log($"{name} HP: {hp}/{maxHP}");

        if (hp <= 0f)
            Destroy(gameObject);
    }
}
