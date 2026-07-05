using System.Collections;
using UnityEngine;

public class StoreInteractScript : MonoBehaviour, IInteractInterface
{
    Vector3 startPos;
    public MainMenu menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(GameObject obj)
    {
        menu.OpenStore();
        return true;
    }

}
