using UnityEngine;

/// <summary>
/// Friendly AI that follows the player and stays near them.
/// </summary>
public class FriendlyAI : AIController
{
    private GameObject player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Think()
    {
        if (state == AIState.Dying)
            return;

        if (player == null)
        {
            state = AIState.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > detectionRange)
            state = AIState.Chase;     // follow player
        else
            state = AIState.Idle;      // stand near player
    }

    protected override void Act()
    {
        switch (state)
        {
            case AIState.Chase:
                FollowPlayer();
                break;

            case AIState.Idle:
                Idle();
                break;

            case AIState.Dying:
                break;
        }
    }

    private void FollowPlayer()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(player.transform.position);
    }

    private void Idle()
    {
        navAgent.isStopped = true;
        animSystem.SetBool("idle", true);
    }
}