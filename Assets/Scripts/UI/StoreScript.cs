using TMPro;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    public TMP_Text classText;
    private PlayerStatSystem playerStats;
    private EClasses curClass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetComponent<PlayerStatSystem>();
        curClass = playerStats.Class;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.Class != curClass)
        {
            curClass = playerStats.Class;
            classText.text = "Current Class: " + curClass.ToString();
        }

    }
}
