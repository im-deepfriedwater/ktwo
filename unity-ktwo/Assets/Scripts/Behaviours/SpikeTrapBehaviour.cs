using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float damage;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            DealDamage(other);
        }
    }

    // void OnTriggerExit(Collider other) 
    // {
    //     if (other.gameObject.tag == "Zombie")
    //     {
    //         other.gameObject.GetComponent<DamagableEnemy>().isInvincible = false;
    //     }
    // }

    void DealDamage (Collider other) 
    {
        var zombie = other.gameObject.GetComponent<DamagableEnemy>();
        zombie.Hit(damage, Vector3.zero);
    }
}
