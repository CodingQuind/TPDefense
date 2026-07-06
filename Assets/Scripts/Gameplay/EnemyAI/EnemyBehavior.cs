using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour, IDamageableInterface
{
    [Header("Systems")]
    private EnemyStatSystem stats;
    private Transform playerLoc;
    private GameObject playerRef;
    private NavMeshAgent navAgent;
    private List<Checkpoint> checkpoints;

    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        stats = gameObject.GetComponent<EnemyStatSystem>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(gameObject.transform.position + (Vector3.up * 2), FindMoveTarget());
    }

    private void FindPlayer() { playerLoc = playerRef.transform; }
    private Vector3 FindMoveTarget()
    {
        GameObject[] possibleDests = GameObject.FindGameObjectsWithTag("EnemyTarget");
        return possibleDests[0].transform.position;
    }
    private void Move(Vector3 source, Vector3 dest)
    {
        transform.LookAt(dest);
        navAgent.destination = dest;
    }

}
