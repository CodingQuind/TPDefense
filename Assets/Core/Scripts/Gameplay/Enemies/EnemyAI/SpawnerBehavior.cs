using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    [SerializeField] private Checkpoint[] checkpoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(GameObject enemyPrefab, int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            GameObject go = Instantiate(enemyPrefab, transform.position, transform.rotation);
            EnemyAI enemyAI = go.GetComponent<EnemyAI>();
            enemyAI.SetCheckpoints(this.checkpoints);
        }
    }
}
