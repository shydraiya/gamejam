using System;

public enum UpgradeType
{
    ShotgunDamage,
    ShotgunFireRate,
    SniperDamage,
    SniperFireRate,
    Heal,
    MoveSpeed,
    ShotgunPelletNum,
    gainMaxHealth,
    Healing,
    Bomb
}

[Serializable]
public class UpgradeOption
{
    public UpgradeType type;
    public string title;
    public string description;
}
