using TMPro;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    public TMP_Text classText;
    private GameObject playerRef;
    private PlayerController controller;
    private EClasses curClass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        controller = playerRef.GetComponent<PlayerController>();
        curClass = controller.GetClass();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetClass() != curClass)
        {
            curClass = controller.GetClass();
            classText.text = "Current Class: " + curClass.ToString();
        }
        
    }
}
