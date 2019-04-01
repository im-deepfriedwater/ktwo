using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammablePuddleBehaviour : BasePuddleBehaviour
{
    public int DPS;
    public int duration;

    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        StartCoroutine(
            other.GetComponent<DamagableEnemy>()
                .DamageOverTime(DPS, duration)
        );
        StartCoroutine(
            other.GetComponent<DamagableEnemy>()
                .SetOnFire(duration)
        );
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
        numberOfUses -= 1;
    }
}
