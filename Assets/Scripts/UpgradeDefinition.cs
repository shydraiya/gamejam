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
    Healing
}

[Serializable]
public class UpgradeOption
{
    public UpgradeType type;
    public string title;
    public string description;
}
