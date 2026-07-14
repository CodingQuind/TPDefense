using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScript : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStatSystem stats;
    public TMP_Text debugTextElement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        stats = controller.gameObject.GetComponent<PlayerStatSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        debugTextElement.text = $"Player: class={controller.Combat.GetClass()}, strength={stats.Strength}, agility={stats.Agility}, " 
            + $"intelligence={stats.Intelligence}, constitution={stats.Constitution}, level={stats.Level}, money={controller.Resource.Money}, "
            + $"\nCurrent Health={stats.CurrentHealth}";
    }
}
