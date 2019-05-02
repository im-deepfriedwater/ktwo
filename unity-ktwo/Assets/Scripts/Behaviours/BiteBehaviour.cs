using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BiteBehaviour : NetworkBehaviour
{
    public int damageAmount;
    public float duration;

    Animator animator;

    void Start()
    {
        if (!isServer)
        {
            animator = GetComponentInParent<Animator>();
            animator.SetBool("IsBiting", true);
        }
        StartCoroutine(
            ResetBiting()
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Zombie") return;
        other.GetComponent<DamagableEnemy>().Hit(damageAmount, gameObject.transform.forward, true);
    }

    private IEnumerator ResetBiting()
    {
        yield return new WaitForSeconds(duration);
        if (!isServer) animator.SetBool("IsBiting", false);
        Destroy(gameObject);
    }
}
