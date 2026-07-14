using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Game Settings")]
    public float gameLength = 500f;
    public int moneyRegenAmt = 10;

    [Header("Character Settings")]
    public CharacterSettings CharacterSettings;

    [Header("Build System Settings")]
    public BuildSystemSettings BuildSystemSettings;

    [Header("Stat System Settings")]
    public StatSystemSettings StatSystemSettings;

    [Header("Enemy Settings")]
    public EnemySettings EnemySettings;

    [Header("AI Settings")]
    public AISettings AISettings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}


[System.Serializable]
public class CharacterSettings
{
    public string characterName;
    public float characterSpeed = 8f;
    public float characterSprintSpeed = 15f;
    public float interactRange = 5f;

    public EClasses characterClass;

    public float attackSpeed = .5f;
    public float attackRange = 3f;
    public float attackDamage = 15f;
}

[System.Serializable]
public class BuildSystemSettings
{
    public float buildRange = 15f;
}

[System.Serializable]
public class StatSystemSettings
{
    public float defaultHealth = 1000;
    public float defaultEnergy = 100;
    public int defaultLevel = 1;
    public int defaultXp = 0;

    public float regenerationRate = 25f;
    public float regenerationDelay = 3f;

    public float defaultEnemyHealth = 100;
    public float defaultEnemyEnergy = 100;
}

[System.Serializable]
public class EnemySettings
{
    public float defaultHealth = 100;
    public float defaultEnergy = 100;
    public int defaultLevel = 1;
    public int defaultXp = 0;

    public float defaultAttackRange = 2f;
    public float defaultAttackDamage = 5f;
    public float defaultSpeed = 3f;
    public float defaultSpeedModifier = 1f;
}

[System.Serializable]
public class AISettings
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 5f;
    public float speed = 3f;
    public float speedModifier = 1f;
}
