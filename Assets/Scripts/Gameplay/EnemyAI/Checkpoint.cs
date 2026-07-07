using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform worldLoc { get; private set; }
    public int priority = 0;

    void Start()
    {
        worldLoc = gameObject.transform;
    }
}
