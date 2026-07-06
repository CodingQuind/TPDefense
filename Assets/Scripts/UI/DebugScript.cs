using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScript : MonoBehaviour
{
    private GameObject playerRef;
    private PlayerController controller;
    private StatSystem stats;
    public TMP_Text debugTextElement;
    private InputAction debugKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        controller = playerRef.GetComponent<PlayerController>();
        stats = playerRef.GetComponent<StatSystem>();
        debugKey = InputSystem.actions.FindAction("Attack");

    }

    // Update is called once per frame
    void Update()
    {
        debugTextElement.text = $"Player: class={controller.GetClass()}, strength={stats.GetStrength()}, agility={stats.GetAgility()}, " 
            + $"intelligence={stats.GetIntelligence()}, constitution={stats.GetConstitution()}, level={stats.GetLevel()}, money={controller.GetMoney()}";
    }
}
