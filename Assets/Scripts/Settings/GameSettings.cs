using UnityEngine;

[System.Serializable]
public static class GameSettings
{
    public static float gameLength; public static float gameSpeed;
}

[System.Serializable]
public static class CharacterSettings
{
    public static string characterName;
    public static float characterSpeed = 5f, characterSprintSpeed = 8f;
    public static ClassEnum characterClass;
}
[System.Serializable]
public static class StatSystemSettings
{
    public static float defaultHealth = 100, defaultEnergy = 100;
    public static int defaultLevel = 1, defaultXp = 0;
}
