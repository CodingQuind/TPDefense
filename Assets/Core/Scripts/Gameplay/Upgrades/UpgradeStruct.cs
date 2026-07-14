using UnityEngine;

[System.Serializable]
public struct UpgradeStruct
{
    public EUpgradeType upgradeType;
    public int upgradeValue;
    public float duration;

    public UpgradeStruct(EUpgradeType type, int value, float duration)
    {
        upgradeType = type;
        this.upgradeValue = value;
        this.duration = duration;
    }
}
