using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("Projectile Settigns")]
    public int damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        other.GetComponent<DamagableEnemy>().Hit(damageAmount);
        transform.localScale = Vector3.zero;
    }
}
