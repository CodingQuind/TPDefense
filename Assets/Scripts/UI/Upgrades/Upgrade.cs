using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public int cost;
    public EUpgradeType upgradeType;
    public float statMultiplier;
    public float cooldownReduction;
    public Sprite icon;
}
