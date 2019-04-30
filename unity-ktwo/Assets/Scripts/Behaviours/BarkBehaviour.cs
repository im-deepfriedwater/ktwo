using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarkBehaviour : NetworkBehaviour
{
    public float duration;

    Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        animator.SetBool("IsBarking", true);
        StartCoroutine(
            ResetBarking()
        );
    }

    void OnTriggerStay(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Zombie") return;
        other.GetComponent<DamagableEnemy>().Hit(0f, -other.transform.forward, true);
    }

    private IEnumerator ResetBarking()
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool("IsBarking", false);
        Destroy(gameObject);
    }
}
