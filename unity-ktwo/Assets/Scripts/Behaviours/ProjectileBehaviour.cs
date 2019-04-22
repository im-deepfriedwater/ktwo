using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileBehaviour : NetworkBehaviour
{
    [Header("Projectile Settigns")]
    public int damageAmount;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hello??");
        Debug.Log(isServer);
        if (!isServer) return;
        if (other.gameObject.tag != "Zombie") return;

        other.GetComponent<DamagableEnemy>().Hit(damageAmount, Vector3.zero, true);
        Debug.Log(other.GetComponent<DamagableEnemy>().currentHealth);
        transform.localScale = Vector3.zero;
    }
}
