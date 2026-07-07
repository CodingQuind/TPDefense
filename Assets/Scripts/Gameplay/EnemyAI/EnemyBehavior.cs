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
    private Collider targetCollider;

    private GameObject playerRef;
    private NavMeshAgent navAgent;
    public List<Checkpoint> checkpoints;
    private int checkpointIndex;
    private float attackTimer = 0f, attackInterval = 1f;

    public enum AIState { Patrol, Chase, Attack }
    public AIState state = AIState.Patrol;

    public float detectionRange = 10f;
    public float attackRange = 1f;

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
        attackTimer += Time.deltaTime;
        GameObject newTarget = GetClosestEnemy();
        if (newTarget != null)
        {
            SetTarget(newTarget);
        }
        else
        {
            currentTarget = null;
            targetCollider = null;
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

                float dist = Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position));

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

                float attackDist = Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position));

                if (attackDist > attackRange)
                {
                    state = AIState.Chase;
                }
                else
                {
                    Attack(currentTarget);
                }
                break;
        }
        //float distance = Vector3.Distance(gameObject.transform.position + (Vector3.up * 2), FindMoveTarget());
        //if (distance > stopDistance)
        //{
        //    Move(gameObject.transform.position + (Vector3.up * 2), FindMoveTarget());
        //} else
        //{
        //    navAgent.isStopped = true;
        //}
    }


    private void Move(Vector3 source, Vector3 dest)
    {
        navAgent.isStopped = false;
        transform.LookAt(dest);
        navAgent.destination = dest;
    }

    private GameObject GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTarget");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            // Find the collider anywhere in the hierarchy
            Collider col = e.GetComponentInParent<Collider>();
            if (col == null)
                continue; // skip objects without colliders

            // Closest point on the collider surface
            Vector3 closestPoint = col.ClosestPoint(transform.position);

            float dist = Vector3.Distance(transform.position, closestPoint);

            if (dist < minDist)
            {
                minDist = dist;
                closest = col.gameObject; // return the object that actually has the collider
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

        // Face the target
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        // Trigger your animation
        var hit = target.GetComponent<IDamageableInterface>();
        if (attackTimer >= attackInterval)
        {
            if (hit != null)
            {
                hit.TakeDamage(this.gameObject, stats.GetPhysicalDamage(), EDamageType.physical);
                animSystem.SetTrigger("attack");
                attackTimer = 0f;
            }
        }
        else animSystem.SetTrigger("attack");
    }


    void SetTarget(GameObject t)
    {
        currentTarget = t;

        // Find the collider anywhere above this object
        targetCollider = t.GetComponentInParent<Collider>();

        // Optional safety check
        if (targetCollider == null)
            Debug.LogWarning($"No collider found for target {t.name}");
    }
}
