using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public PlayerWeaponController wc;
    public PlayerStats stats;
    public Shotgun shotgun;
    public Sniper sniper;

    List<UpgradeOption> pool = new()
    {
        new UpgradeOption { type = UpgradeType.ShotgunDamage, title="샷건 데미지 +20%" },
        new UpgradeOption { type = UpgradeType.ShotgunFireRate, title="샷건 발사속도 +15%" },
        new UpgradeOption { type = UpgradeType.SniperDamage, title="스나이퍼 데미지 +30%" },
        new UpgradeOption { type = UpgradeType.SniperFireRate, title="스나이퍼 재사용 -20%" },
        new UpgradeOption { type = UpgradeType.Heal, title="체력 회복" },
        new UpgradeOption { type = UpgradeType.MoveSpeed, title="이동속도 +10%" },
        new UpgradeOption { type = UpgradeType.ShotgunPelletNum, title="샷건 탄환수 +2"},
        new UpgradeOption { type = UpgradeType.gainMaxHealth, title="최대 체력 +30"},
        new UpgradeOption { type = UpgradeType.Healing, title="지속 치유 +1/s"},
        new UpgradeOption { type = UpgradeType.Bomb, title="주변 적 즉사"}
    };

    public List<UpgradeOption> GetRandomOptions(int count)
    {
        List<UpgradeOption> copy = new(pool);
        List<UpgradeOption> result = new();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int idx = Random.Range(0, copy.Count);
            result.Add(copy[idx]);
            copy.RemoveAt(idx);
        }
        return result;
    }

    public void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.ShotgunDamage:
                shotgun.damagePerPellet *= 1.2f;
                break;

            case UpgradeType.ShotgunFireRate:
                shotgun.fireInterval *= 0.85f;
                break;

            case UpgradeType.SniperDamage:
                sniper.damagePerPellet *= 1.3f;
                break;

            case UpgradeType.SniperFireRate:
                sniper.fireInterval *= 0.8f;
                break;

            case UpgradeType.Heal:
                stats.Heal(30);
                break;

            case UpgradeType.MoveSpeed:
                stats.AddMoveSpeedMultiplier(0.10f);
                break;

            case UpgradeType.ShotgunPelletNum:
                shotgun.pellets += 2;
                break;

            case UpgradeType.gainMaxHealth:
                stats.maxHP += 30;
                stats.Heal(30);
                break;
            
            case UpgradeType.Healing:
                stats.AddHpRegen(1);
                break;
            
            case UpgradeType.Bomb:
                wc.KillEnemiesInRange();
                break;
        }
    }
}
