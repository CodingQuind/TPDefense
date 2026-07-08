using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class EnemyBehavior : MonoBehaviour, IDamageableInterface
{
    [Header("Systems")]
    [SerializeField] private Animator animSystem;
    private EnemyStatSystem stats;
    private GameObject currentTarget;

    private GameObject playerRef;
    private NavMeshAgent navAgent;
    public List<Checkpoint> checkpoints;
    private int checkpointIndex;
    private float attackTimer = 0f, attackInterval = 2f;

    public enum AIState { Patrol, Chase, Attack, Dying, Defending }
    public AIState state = AIState.Patrol;

    public float detectionRange = 10f;
    public float attackRange = 1f;

    public void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        if (stats.Damage(damage))
        {
            KillSelf();
        }
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
        attackTimer += Time.deltaTime;
        GameObject newTarget = GetClosestEnemy();
        if (newTarget != null)
        {
            SetTarget(newTarget);
        }
        else
        {
            currentTarget = null;
        }

        switch (state)
        {
            case AIState.Patrol:
                Patrol();

                if (currentTarget != null &&
                    Vector3.Distance(transform.position, currentTarget.transform.position) < detectionRange)
                {
                    state = AIState.Chase;
                }
                break;

            case AIState.Chase:
                if (currentTarget == null)
                {
                    state = AIState.Patrol;
                    break;
                }

                float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (dist > detectionRange)
                {
                    state = AIState.Patrol;
                }
                else if (dist <= attackRange)
                {
                    state = AIState.Attack;
                }
                else
                {
                    Chase(currentTarget);
                }
                break;

            case AIState.Attack:
                if (currentTarget == null)
                {
                    state = AIState.Patrol;
                    break;
                }

                float attackDist = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (attackDist > attackRange)
                {
                    state = AIState.Chase;
                }
                else
                {
                    Attack(currentTarget);
                }
                break;
            case AIState.Dying:
                break;
        }
    }

    private GameObject GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTarget");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            var eTransform = e.transform.parent.transform;
            float dist = Vector3.Distance(transform.position, eTransform.transform.position);

            if (dist < minDist && e.activeSelf == true)
            {
                minDist = dist;
                closest = e.transform.parent.gameObject;
            }
        }
        return closest;
    }

    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(checkpoints[checkpointIndex].worldLoc.position);

        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            checkpointIndex = (checkpointIndex + 1) % checkpoints.Count;
        }
    }

    void Chase(GameObject target)
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(target.transform.position);
    }

    void Attack(GameObject target)
    {
        navAgent.isStopped = true;
        var hit = target.GetComponent<IDamageableInterface>();
        if (attackTimer >= attackInterval && target.activeSelf == true)
        {
            if (hit != null)
            {
                hit.TakeDamage(this.gameObject, stats.GetPhysicalDamage(), EDamageType.physical);
                Debug.Log("[EnemyBehavior] DEBUG: hitting target for " + stats.GetPhysicalDamage() + " damage.");
                animSystem.SetTrigger("attack");
                attackTimer = 0f;
            }
        }
        
    }


    void SetTarget(GameObject t)
    {
        currentTarget = t;
    }

    public void SetCheckpoints(Checkpoint[] checkpoints)
    {
        this.checkpoints = new();
        foreach(var check in checkpoints)
        {
            this.checkpoints.Add(check);
        }
    }

    private void KillSelf()
    {
        animSystem.SetTrigger("dead");
        navAgent.enabled = false;
        state = AIState.Dying;

        Destroy(gameObject, 3f);
    }
}
