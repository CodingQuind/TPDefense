using UnityEngine;

[System.Serializable]
public struct UpgradeStruct
{
    public EUpgradeType upgradeType;
    public float statMultiplier;
    public float duration;

    public UpgradeStruct(EUpgradeType type, float multiplier, float duration)
    {
        upgradeType = type;
        this.statMultiplier = multiplier;
        this.duration = duration;
    }
}
