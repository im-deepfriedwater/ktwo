using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammablePuddleBehaviour : MonoBehaviour
{

    public int numberOfUses;
    public int DPS;
    public int duration;

    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();
    
    void OnTriggerEnter(Collider other)
    {
        if (affectedEntities.Contains(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        // if (numberOfUses == 0)
        // {
        //     Destroy(gameObject);
        // }
        // numberOfUses -= 1;

        // Deals 10 DPS for 5 seconds
        StartCoroutine(
            other.GetComponent<DamagableEnemy>().
            DamageOverTime(DPS, duration, affectedEntities)
        );
    }
}
