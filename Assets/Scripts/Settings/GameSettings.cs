using UnityEngine;

[System.Serializable]
public static class GameSettings
{
    public static float gameLength = 500f; //Defines in seconds how long the game should last before you win
    public static int moneyRegenAmt = 10; //Defines how much money should be generate per s
}

[System.Serializable]
public static class CharacterSettings
{
    public static string characterName;
    public static float characterSpeed = 5f, characterSprintSpeed = 8f, interactRange = 5f;
    public static EClasses characterClass;
    public static float attackSpeed = 1f, attackRange = 2f, attackDamage = 10f;
}

[System.Serializable]
public static class BuildSystemSettings
{
    public static float buildRange = 25f;
}
[System.Serializable]
public static class StatSystemSettings
{
    public static float defaultHealth = 1000, defaultEnergy = 100;
    public static int defaultLevel = 1, defaultXp = 0;
    public static float regenerationRate = 25f, regenerationDelay = 3f;
    public static float defaultEnemyHealth = 100, defaultEnemyEnergy = 100;
}

[System.Serializable]
public static class EnemySettings
{
    public static float defaultHealth = 100, defaultEnergy = 100;
    public static int defaultLevel = 1, defaultXp = 0;
    public static float defaultAttackRange = 2f, defaultAttackDamage = 5f, defaultSpeed = 3f, defaultSpeedModifier = 1f;
}

[System.Serializable]
public static class AISettings
{
    public static float detectionRange = 10f, attackRange = 2f, attackDamage = 5f, speed = 3f, speedModifier = 1f;
}
