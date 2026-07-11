using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Base class for all AI agents. Handles shared systems such as movement,
/// animation, targeting, state transitions, and stat initialization.
/// </summary>
public abstract class AIController : MonoBehaviour, IDamageableInterface
{
    [Header("Systems")]
    [SerializeField] protected Animator animSystem;
    protected BaseStatSystem stats;
    protected NavMeshAgent navAgent;
    protected float moveSpeed = AISettings.speed;
    [SerializeField] protected HealthBar healthbar;

    protected GameObject currentTarget;
    protected float attackTimer;
    protected float attackInterval = 2f;

    public enum AIState { Patrol, Chase, Attack, Dying, Idle, Defending }
    public AIState state = AIState.Patrol;

    [Header("Ranges")]
    public float detectionRange = AISettings.detectionRange;
    public float attackRange = AISettings.attackRange;

    protected virtual void Start()
    {
        stats = GetComponent<BaseStatSystem>();
        navAgent = GetComponent<NavMeshAgent>();

        stats.InitializeStats();
        if (navAgent != null) navAgent.speed = moveSpeed;
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        Think();
        Act();
        healthbar.SetHealth(stats.CurrentHealth, stats.MaxHealth);
    }

    /// <summary>
    /// Determines what the AI should do next (state logic).
    /// </summary>
    protected abstract void Think();

    /// <summary>
    /// Executes the behavior for the current state.
    /// </summary>
    protected abstract void Act();

    /// <summary>
    /// Called when the AI takes damage.
    /// </summary>
    public virtual void TakeDamage(GameObject attacker, float damage, EDamageType damageType)
    {
        if (stats.Damage(damage))
        {
            KillSelf();
        }
        healthbar.UpdateHealth(stats.CurrentHealth, stats.MaxHealth);
    }

    public virtual void DamageTarget(GameObject target, float damage, EDamageType damageType)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Sets the current target for the AI.
    /// </summary>
    protected void SetTarget(GameObject t)
    {
        currentTarget = t;
    }

    /// <summary>
    /// Plays death animation and schedules destruction.
    /// </summary>
    protected virtual void KillSelf()
    {
        animSystem.SetTrigger("dead");
        navAgent.enabled = false;
        state = AIState.Dying;
        gameObject.tag = "Untagged";
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3f);
    }

    protected virtual void MoveTo(Vector3 position)
    {
        if (navAgent == null) return;
        navAgent.isStopped = false;
        navAgent.SetDestination(position);
    }

    protected virtual void StopMovement()
    {
        if (navAgent == null) return;
        navAgent.isStopped = true;
    }
}
