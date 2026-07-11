using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject weaponRoot;
    private Animator animator;

    void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAttackAnimation(string weaponType) //Redo with enumerator for weapon types and play the appropriate animation based on the weapon type.
    {
        StartCoroutine(PlaceholderAnimation());
    }

    private IEnumerator PlaceholderAnimation()
    {
        float elapsedTime = 0f;
        Quaternion origRotation = weaponRoot.transform.localRotation;
        while (elapsedTime < CharacterSettings.attackSpeed)
        {
            weaponRoot.transform.localRotation = Quaternion.Euler(Mathf.Sin(elapsedTime * Mathf.PI * 2) * 30, 0, 0); // Placeholder rotation animation
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        weaponRoot.transform.localRotation = origRotation;
    }
}

