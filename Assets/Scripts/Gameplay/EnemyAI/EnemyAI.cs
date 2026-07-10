using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Enemy AI implementing patrol, chase, and attack behaviors.
/// </summary>
public class EnemyAI : AIController
{
    public List<Checkpoint> checkpoints = new();
    private int checkpointIndex;

    protected override void Think()
    {
        GameObject newTarget = GetClosestEnemy();
        if (newTarget != null)
            SetTarget(newTarget);
        else
            currentTarget = null;

        switch (state)
        {
            case AIState.Patrol:
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
                    state = AIState.Patrol;
                else if (dist <= attackRange)
                    state = AIState.Attack;

                break;

            case AIState.Attack:
                if (currentTarget == null)
                {
                    state = AIState.Patrol;
                    break;
                }

                float attackDist = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (attackDist > attackRange)
                    state = AIState.Chase;

                break;
        }
    }

    protected override void Act()
    {
        switch (state)
        {
            case AIState.Patrol:
                Patrol();
                break;

            case AIState.Chase:
                Chase(currentTarget);
                break;

            case AIState.Attack:
                Attack(currentTarget);
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
            float dist = Vector3.Distance(transform.position, eTransform.position);

            if (dist < minDist && e.activeSelf)
            {
                minDist = dist;
                closest = e.transform.parent.gameObject;
            }
        }
        return closest;
    }

    private void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(checkpoints[checkpointIndex].worldLoc.position);

        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            checkpointIndex = (checkpointIndex + 1) % checkpoints.Count;
        }
    }

    private void Chase(GameObject target)
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(target.transform.position);
    }

    private void Attack(GameObject target)
    {
        navAgent.isStopped = true;

        if (attackTimer < attackInterval || !target.activeSelf)
            return;

        if (target.TryGetComponent<IDamageableInterface>(out var hit))
        {
            hit.TakeDamage(gameObject, stats.GetPhysicalDamage(), EDamageType.physical);
            animSystem.SetTrigger("attack");
            attackTimer = 0f;
        }
    }

    public void SetCheckpoints(Checkpoint[] checkpoints)
    {
        this.checkpoints = new();
        foreach (var check in checkpoints)
            this.checkpoints.Add(check);
    }
}

