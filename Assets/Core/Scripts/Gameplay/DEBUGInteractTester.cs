using System.Collections;
using UnityEngine;

public class DEBUGInteractTester : MonoBehaviour, IInteractInterface
{
    Vector3 startPos;
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
        BounceObject();
        return true;
    }

    private void BounceObject()
    {
        StartCoroutine(BounceRoutine());
    }

    private IEnumerator BounceRoutine()
    {
        float duration = 0.15f;
        float height = 0.5f;

        Vector3 endPos = startPos + Vector3.up * height;

        // Bounce up
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Bounce down
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }
    }
}
