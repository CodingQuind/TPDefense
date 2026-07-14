using UnityEngine;

public class PlayerResourceController : MonoBehaviour
{
    public int Money { get; private set; } = 0;
    public int Wood { get; private set; } = 0;
    public int Stone { get; private set; } = 0;

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SpendMoney(int amount)
    {
        Money = Mathf.Max(0, Money - amount);
    }

    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }
}
