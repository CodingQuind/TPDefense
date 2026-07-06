using UnityEngine;

public class AnimaterScript : MonoBehaviour
{
    Animator anim;
    CharacterController controller;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        float speed = controller.velocity.magnitude;
        anim.SetFloat("speed", speed);
    }
}
