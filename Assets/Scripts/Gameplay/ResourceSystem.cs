using UnityEngine;

[System.Serializable]
public class ResourceSystem : MonoBehaviour
{
    public int startingMoney = 50;
    private int money, moneyRegenRate = GameSettings.moneyRegenAmt;
    public void Start() { money = startingMoney; }
    public void AddMoney(int amt) { money += amt; }
    public bool RemoveMoney(int amt) { return __RemoveMoney(amt); }
    private bool __RemoveMoney(int amt)
    {
        if (money - amt < 0)
        {
            return false;
        }
        money -= amt;
        return true;
    }
    public int Money() { return money; }
}
