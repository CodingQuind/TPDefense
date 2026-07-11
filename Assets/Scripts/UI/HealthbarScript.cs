using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;           // The enemy
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Image fillImage;            // The red/green fill
    private Camera cam;

    void Start()
    {
        

    }

    void LateUpdate()
    {
        if (cam != null)
        {
            // Follow the target
            transform.position = target.position + offset;

            // Face the camera
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }

    public void SetHealth(float current, float max)
    {
        cam = Camera.main;
        StartCoroutine(AnimateHealth(current, max));
    }

    public void UpdateHealth(float current, float max)
    {
        StartCoroutine(AnimateHealth(current, max));
    }

    public IEnumerator AnimateHealth(float newHealth, float maxHealth)
    {
        float start = fillImage.fillAmount;
        float end = newHealth / maxHealth;
        float t = 0f;

        while (t < 0.2f)
        {
            t += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(start, end, t / 0.2f);
            yield return null;
        }

        fillImage.fillAmount = end;
    }
}