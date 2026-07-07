using UnityEngine;
using UnityEngine.AI;

public class AnimaterScript : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float speed = agent.velocity.magnitude;
        anim.SetFloat("speed", speed);
    }
}
