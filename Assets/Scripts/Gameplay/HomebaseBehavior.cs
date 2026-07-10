using UnityEngine;

public class HomebaseBehavior : MonoBehaviour, IDamageableInterface
{
    private PlayerController playerRef;
    private float lastAttackedTimer = 0f, lastAttackedBroadcastThreshold = 10f;
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; } = 1000f;

    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        // Later use specific resistances
        if (lastAttackedTimer > lastAttackedBroadcastThreshold)
        {
            //playerRef.SendAlertToHud("Alert: Homebase under attack!");
        }
        lastAttackedTimer = 0f;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamemode>().GameOver();
        }
        else if (currentHealth < maxHealth / 2)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //playerRef.SendAlertToHud("Alert: Homebase health below 50%, enemies have been wiped");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lastAttackedTimer += Time.deltaTime;
    }
}
