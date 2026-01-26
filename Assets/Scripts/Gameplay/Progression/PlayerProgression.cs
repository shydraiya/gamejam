using System;

using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    public LevelUpUI levelUpUI;
    public int level = 1;
    public float xp = 0;
    public float xpToLevelUp = 10;

    public event Action OnLevelUp;

    public void AddXP(float amount)
    {
        xp += amount;
        while (xp >= xpToLevelUp)
        {
            xp -= xpToLevelUp;
            level++;
            xpToLevelUp *= 2.0f;
            OnLevelUp?.Invoke();
        }
    }

    void Start()
    {
        
    }


}
